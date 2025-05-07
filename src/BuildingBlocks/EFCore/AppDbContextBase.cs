namespace BuildingBlocks.EFCore;

using System.Collections.Immutable;
using Core.Model;
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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var entry in ChangeTracker.Entries<IEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.CreatedBy = 1; // Default system user ID
                        entry.Entity.LastModified = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = 1; // Default system user ID
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = 1; // Default system user ID
                        entry.Entity.Version++;
                        break;

                    case EntityState.Deleted:
                        if (entry.Entity is ISoftDeletable softDeletableEntity)
                        {
                            entry.State = EntityState.Modified;
                            softDeletableEntity.LastModified = DateTime.UtcNow;
                            softDeletableEntity.LastModifiedBy = 1; // Default system user ID
                            softDeletableEntity.IsDeleted = true;
                            softDeletableEntity.DeletedAt = DateTime.UtcNow;
                            softDeletableEntity.DeletedBy = 1; // Default system user ID
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
}
