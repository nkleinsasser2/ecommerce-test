namespace BuildingBlocks.EFCore;

using System.Collections.Immutable;
using Core.Model;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Core.Event;
using Exception = System.Exception;

public abstract class AppDbContextBase : DbContext, IDbContext
{
    protected AppDbContextBase(DbContextOptions options) :
        base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        return await base.SaveChangesAsync(cancellationToken);
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

    // ref: https://www.meziantou.net/entity-framework-core-generate-tracking-columns.htm
    // ref: https://www.meziantou.net/entity-framework-core-soft-delete-using-query-filters.htm
    private void OnBeforeSaving()
    {
        try
        {
            foreach (var entry in ChangeTracker.Entries<IEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.Now;
                        entry.Entity.CreatedBy = 1; // Default system user ID
                        entry.Entity.LastModified = DateTime.Now; // Set LastModified on creation
                        entry.Entity.LastModifiedBy = 1; // Set LastModifiedBy on creation
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.Now;
                        entry.Entity.LastModifiedBy = 1; // Default system user ID
                        entry.Entity.Version++;
                        break;

                    case EntityState.Deleted:
                        if (entry.Entity is ISoftDeletable softDeletableEntity)
                        {
                            entry.State = EntityState.Modified;
                            softDeletableEntity.LastModified = DateTime.Now;
                            softDeletableEntity.LastModifiedBy = 1; // Default system user ID
                            softDeletableEntity.IsDeleted = true;
                            softDeletableEntity.Version++;
                        }
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
