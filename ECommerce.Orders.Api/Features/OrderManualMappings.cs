using ECommerce.Infrastructure.Inventories.Models;
using ECommerce.Infrastructure.Orders.Dtos;
using ECommerce.Infrastructure.Orders.Models;
using ECommerce.Infrastructure.Orders.ValueObjects;
using ECommerce.Infrastructure.Products.ValueObjects;
using MassTransit;
using System;

namespace ECommerce.Orders.Api.Features;
public static class OrderManualMappings
{
    public static IEnumerable<OrderItem> MapTo(this IEnumerable<ItemDto> items, OrderId orderId,
        IEnumerable<InventoryItems> inventoryItems)
    {
        return items?.Select(x =>
        {
            var now = DateTime.UtcNow;
            var systemUser = 1;

            return new OrderItem
            {
                Id = OrderItemId.Of(NewId.NextGuid()),
                Quantity = Quantity.Of(x.Quantity),
                ProductId = ProductId.Of(x.ProductId),
                Product = inventoryItems.FirstOrDefault(pi => pi.ProductId == ProductId.Of(x.ProductId))?.Product,
                OrderId = orderId,
                
                CreatedAt = now,
                CreatedBy = systemUser,
                LastModified = now,
                LastModifiedBy = systemUser,
                Version = 0,
                IsDeleted = false
            };
        });
    }
}
