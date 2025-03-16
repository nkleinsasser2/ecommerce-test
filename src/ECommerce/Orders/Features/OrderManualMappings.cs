
using ECommerce.Infrastructure.Inventories.Models;
using ECommerce.Infrastructure.Orders.Dtos;
using ECommerce.Infrastructure.Orders.Models;
using ECommerce.Infrastructure.Orders.ValueObjects;
using ECommerce.Infrastructure.Products.ValueObjects;
using MassTransit;

namespace ECommerce.Orders.Features;
public static class OrderManualMappings
{
    public static IEnumerable<OrderItem> MapTo(this IEnumerable<ItemDto> items, OrderId id,
        IEnumerable<InventoryItems> inventoryItems)
    {
        return items?.Select(x => new OrderItem
        {
            Id = OrderItemId.Of(NewId.NextGuid()),
            Quantity = Quantity.Of(x.Quantity),
            ProductId = ProductId.Of(x.ProductId),
            Product = inventoryItems.FirstOrDefault(x => x.ProductId == ProductId.Of(x.ProductId))?.Product,
            OrderId = id
        });
    }
}
