namespace ECommerce.Infrastructure.Orders.Models;

using BuildingBlocks.Core.Model;
using Contracts;
using Customers.Models;
using Customers.ValueObjects;
using Dtos;
using ECommerce.Infrastructure.Customers.Models;
using ECommerce.Infrastructure.Orders.Contracts;
using ECommerce.Infrastructure.Orders.Events;
using ECommerce.Infrastructure.Orders.ValueObjects;
using Enums;
using Exceptions;
using ValueObjects;

public record Order : Aggregate<OrderId>
{
    private readonly List<OrderItem> _orderItems = new List<OrderItem>();
    public CustomerId CustomerId { get; private set; }
    public Customer Customer { get; private set; }
    public OrderStatus Status { get; private set; }
    public TotalPrice TotalPrice { get; private set; }
    public OrderDate OrderDate { get; private set; }

    public static Order Create(OrderId id, Customer customer, DiscountType discountType,
        decimal discountValue,
        OrderDate? orderDate,
        bool isDeleted = false)
    {
        var order = new Order
        {
            Id = id,
            Customer = customer,
            CustomerId = customer.Id,
            TotalPrice = TotalPrice.Of(0),
            Status = OrderStatus.Pending,
            OrderDate = orderDate,
            IsDeleted = isDeleted
        };

        var @event = new OrderInitialedDomainEvent(order.Id, order.CustomerId, discountType, discountValue,
            order.Status, order.IsDeleted);

        order.AddDomainEvent(@event);

        return order;
    }

    public void AddItems(IEnumerable<OrderItem> items)
    {
        if (items is null || !items.Any())
            throw new InvalidOrderQuantityException();

        _orderItems.AddRange(items);

        var itemsDto = items?.Select(x => new OrderItemDto(x.Id, x.ProductId, x.OrderId, x.Quantity))
            .ToList();

        var @event = new OrderItemsAddedToOrderDomainEvent(itemsDto);

        AddDomainEvent(@event);
    }

    public void ApplyDiscount(DiscountType discountType, decimal discountValue)
    {
        var discountStrategy = DiscountStrategyFactory.CreateDiscountStrategy(discountType, discountValue);

        if (discountStrategy != null)
        {
            TotalPrice -= TotalPrice.Of(discountStrategy.ApplyDiscount(TotalPrice.Value));

            var @event = new OrderDiscountAppliedDomainEvent(Id, CustomerId, discountType, discountValue,
                Status, IsDeleted);

            AddDomainEvent(@event);
        }
    }

    public (IEnumerable<OrderItemDto> ExpressShipmentItems, IEnumerable<OrderItemDto> RegularShipmentItems)
        ApplyShipment()
    {
        var regularItems = new List<OrderItem>();
        var expressItems = new List<OrderItem>();

        var shipmentRegularPostStrategy = ShipmentStrategyFactory.CreateShipmentStrategy(ShipmentType.RegularPost);

        if (shipmentRegularPostStrategy != null)
        {
            regularItems = shipmentRegularPostStrategy.GetOrderItemsToShip(_orderItems);
            if (regularItems.Any())
                TotalPrice += TotalPrice.Of(shipmentRegularPostStrategy.GetShipmentPriceُ());
        }

        var shipmentExpressPostStrategy = ShipmentStrategyFactory.CreateShipmentStrategy(ShipmentType.ExpressPost);

        if (shipmentExpressPostStrategy != null)
        {
            expressItems = shipmentExpressPostStrategy.GetOrderItemsToShip(_orderItems);
            if (expressItems.Any())
                TotalPrice += TotalPrice.Of(shipmentExpressPostStrategy.GetShipmentPriceُ());
        }

        var regularItemsDto = regularItems?.Select(x => new OrderItemDto(x.Id, x.ProductId, x.OrderId, x.Quantity))
            .ToList();
        var expressItemsDto = expressItems?.Select(x => new OrderItemDto(x.Id, x.ProductId, x.OrderId, x.Quantity))
            .ToList();

        var @event = new OrderShipmentAppliedDomainEvent(Id, CustomerId, regularItemsDto, expressItemsDto,
            Status, IsDeleted);

        AddDomainEvent(@event);

        return (expressItemsDto, regularItemsDto);
    }

    public void CalculateTotalPrice()
    {
        TotalPrice = TotalPrice.Of(_orderItems.Sum(item => item.CalculatePrice()));

        // if (TotalPrice.Value < 50000) // Commenting out potentially incorrect validation
        //     throw new InvalidTotalPriceRangeException(TotalPrice.Value);

        var @event = new OrderTotalPriceAddedDomainEvent(Id.Value, CustomerId.Value, OrderDate,
            TotalPrice.Value,
            Status, _orderItems?.Select(x => new OrderItemDto(x.Id, x.ProductId, x.OrderId, x.Quantity)),
            IsDeleted);

        AddDomainEvent(@event);
    }
}
