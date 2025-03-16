using BuildingBlocks.Core.Event;

namespace ECommerce.Infrastructure.Categories.Events;

public record CategoryCreatedDomainEvent(Guid Id, string Name, bool IsDeleted) : IDomainEvent;
