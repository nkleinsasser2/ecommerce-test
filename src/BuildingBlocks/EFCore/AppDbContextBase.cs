namespace BuildingBlocks.EFCore;

using System.Collections.Immutable;
using BuildingBlocks.Core.Model;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Core.Event;
using Exception = System.Exception;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public abstract class AppDbContextBase : DbContext, IDbContext
{
    protected AppDbContextBase(DbContextOptions options) :
        base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

    //ref: https://learn.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency#execution-strategies-and-transactions
    public Task ExecuteTransactionalAsync(CancellationToken cancellationToken = default)
    {
        var strategy = Database.CreateExecutionStrategy();
        return strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
            try
            {
                await SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }

    public override int SaveChanges()
    {
        OnBeforeSaving();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        return base.SaveChangesAsync(cancellationToken);
    }

    //ref: https://www.meziantou.net/entity-framework-core-soft-delete-using-query-filters.htm
    private void OnBeforeSaving()
    {
        var entries = ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted)
            .ToList();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    OnBeforeAdd(entry);
                    break;

                case EntityState.Modified:
                    OnBeforeModify(entry);
                    break;

                case EntityState.Deleted:
                    OnBeforeDelete(entry);
                    break;
            }
        }
    }

    private static void OnBeforeAdd(EntityEntry entry)
    {
        if (entry.Entity is not ISoftDeletable) return;

        entry.CurrentValues[nameof(ISoftDeletable.IsDeleted)] = false;
        entry.CurrentValues[nameof(ISoftDeletable.DeletedAt)] = null;
        entry.CurrentValues[nameof(ISoftDeletable.DeletedBy)] = null;
        entry.CurrentValues[nameof(ISoftDeletable.LastModified)] = DateTime.Now;
        entry.CurrentValues[nameof(ISoftDeletable.LastModifiedBy)] = null;
        entry.CurrentValues[nameof(ISoftDeletable.Version)] = 1;
    }

    private static void OnBeforeModify(EntityEntry entry)
    {
        if (entry.Entity is not ISoftDeletable) return;

        entry.CurrentValues[nameof(ISoftDeletable.LastModified)] = DateTime.Now;
        entry.CurrentValues[nameof(ISoftDeletable.LastModifiedBy)] = null;
        entry.CurrentValues[nameof(ISoftDeletable.Version)] = (long)entry.CurrentValues[nameof(ISoftDeletable.Version)] + 1;
    }

    private static void OnBeforeDelete(EntityEntry entry)
    {
        if (entry.Entity is not ISoftDeletable) return;

        try
        {
            entry.State = EntityState.Modified;
            entry.CurrentValues[nameof(ISoftDeletable.IsDeleted)] = true;
            entry.CurrentValues[nameof(ISoftDeletable.DeletedAt)] = DateTime.Now;
            entry.CurrentValues[nameof(ISoftDeletable.DeletedBy)] = null;
        }
        catch (Exception ex)
        {
            // Log the error
        }
    }

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        var domainEntities = ChangeTracker
            .Entries<IAggregate>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.DomainEvents)
            .ToImmutableList();

        domainEntities.ForEach(entity => entity.ClearDomainEvents());

        return domainEvents.ToImmutableList();
    }
}
