namespace ECommerce.Infrastructure.Orders.Models;

using BuildingBlocks.Core.Model;
using ECommerce.Infrastructure.Orders.ValueObjects;
using ECommerce.Infrastructure.Products.Models;
using ECommerce.Infrastructure.Products.ValueObjects;
using ValueObjects;

public class OrderItem : Entity<OrderItemId>
{
    public ProductId ProductId { get; init; }
    public Product Product { get; init; }
    public OrderId OrderId { get; init; }
    public Order Order { get; init; }
    public Quantity Quantity { get; init; }

    public static OrderItem Create(OrderItemId id, OrderId orderId, ProductId productId, Quantity quantity)
    {
        return new OrderItem
        {
            Id = id,
            OrderId = orderId,
            ProductId = productId,
            Quantity = quantity
        };
    }

    public decimal CalculatePrice()
    {
        return Product.NetPrice.Value * Quantity.Value;
    }
}
