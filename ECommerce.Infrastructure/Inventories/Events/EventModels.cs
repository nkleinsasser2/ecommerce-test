using BuildingBlocks.Core.Event;
using ECommerce.Infrastructure.Inventories.Enums;

namespace ECommerce.Infrastructure.Inventories.Events;

public record ProductAddedToInventoryDomainEvent
    (Guid Id, Guid InventoryId, Guid ProductId, ProductStatus Status, int Quantity) : IDomainEvent;

public record ProductUpdatedToInventoryDomainEvent
    (Guid Id, Guid InventoryId, Guid ProductId, ProductStatus Status, int Quantity) : IDomainEvent;

public record ProductSoldDomainEvent
    (Guid Id, Guid InventoryId, Guid ProductId, ProductStatus Status, int Quantity) : IDomainEvent;


public record ProductDamagedDomainEvent
    (Guid Id, Guid InventoryId, Guid ProductId, ProductStatus Status, int Quantity) : IDomainEvent;
