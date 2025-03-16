using ECommerce.Infrastructure.Customers.ValueObjects;
using ECommerce.Infrastructure.Products.ValueObjects;

namespace Unit.Test.Fakes;

using Common;
using ECommerce.Infrastructure.Orders.Enums;
using ECommerce.Infrastructure.Orders.Models;
using ECommerce.Infrastructure.Orders.ValueObjects;
using ECommerce.Orders.Enums;
using ECommerce.Orders.Models;
using ECommerce.Orders.ValueObjects;
using MassTransit;
using CustomerName = ECommerce.Infrastructure.Customers.ValueObjects.Name;
using ProductName = Name;

public sealed class FakeOrder
{
    public static Order Generate()
    {
        var product = DbContextFactory.Products.First();
        var customer = DbContextFactory.Customers.First();

        var orderId = OrderId.Of(NewId.NextGuid());

        var order = Order.Create(OrderId.Of(orderId), customer
            , DiscountType.AmountDiscount, 1800, OrderDate.Of(new DateTime(2023, 1, 1, 18, 0, 0)));

        order.AddItems(new List<OrderItem>(new[]
        {
            new OrderItem
            {
                Id = OrderItemId.Of(NewId.NextGuid()),
                OrderId = OrderId.Of(orderId),
                Product = product,
                ProductId = product.Id,
                Quantity = Quantity.Of(2)
            }
        }));

        return order;
    }
}
