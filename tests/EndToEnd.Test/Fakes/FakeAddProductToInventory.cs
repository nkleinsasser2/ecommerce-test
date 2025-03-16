namespace EndToEnd.Test.Fakes;

using ECommerce.Infrastructure.Inventories.Enums;
using ECommerce.Infrastructure.Inventories.Models;
using ECommerce.Infrastructure.Inventories.ValueObjects;
using ECommerce.Infrastructure.Products.ValueObjects;
using ECommerce.Inventories.Enums;
using ECommerce.Inventories.Models;
using ECommerce.Inventories.ValueObjects;
using ECommerce.Products.ValueObjects;
using MassTransit;

public class FakeAddProductToInventory
{
    public static InventoryItems Generate(ProductStatus status)
    {
        return InventoryItems.AddProductToInventory(InventoryItemsId.Of(NewId.NextGuid()),
            InventoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c4")), ProductId.Of(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c0")), Quantity.Of(2), status);
    }
}
