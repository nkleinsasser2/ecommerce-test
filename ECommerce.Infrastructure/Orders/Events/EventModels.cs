using BuildingBlocks.Core.Event;
using ECommerce.Infrastructure.Orders.Dtos;
using ECommerce.Infrastructure.Orders.Enums;

namespace ECommerce.Infrastructure.Orders.Events;



public record NewOrderRegisteredDomainEvent
    (Guid Id, Guid InventoryId, Guid ProductId, int Quantity, OrderStatus Status) : IDomainEvent;

public record OrderInitialedDomainEvent
(Guid Id, Guid CustomerId, DiscountType DiscountType, decimal DiscountValue,
    OrderStatus Status = OrderStatus.Pending, bool isDeleted = false) : IDomainEvent;

public record OrderDiscountAppliedDomainEvent
(Guid Id, Guid CustomerId, DiscountType DiscountType, decimal DiscountValue,
    OrderStatus Status, bool isDeleted = false) : IDomainEvent;

public record OrderShipmentAppliedDomainEvent
(Guid Id, Guid CustomerId, IEnumerable<OrderItemDto> RegularOrderItems, IEnumerable<OrderItemDto> ExpressOrderItems,
    OrderStatus Status, bool isDeleted = false) : IDomainEvent;

public record OrderTotalPriceAddedDomainEvent
(Guid Id, Guid CustomerId, DateTime OrderDate, decimal TotalPrice,
    OrderStatus Status, IEnumerable<OrderItemDto> OrderItems, bool isDeleted = false) : IDomainEvent;

public record OrderItemsAddedToOrderDomainEvent(IEnumerable<OrderItemDto> OrderItems) : IDomainEvent;
