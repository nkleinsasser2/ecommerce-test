using ECommerce.Infrastructure.Categories.Models;
using ECommerce.Infrastructure.Categories.ValueObjects;
using ECommerce.Infrastructure.Customers.Models;
using ECommerce.Infrastructure.Customers.ValueObjects;
using ECommerce.Infrastructure.Inventories.Enums;
using ECommerce.Infrastructure.Inventories.Models;
using ECommerce.Infrastructure.Inventories.ValueObjects;
using ECommerce.Infrastructure.Products.Models;
using ECommerce.Infrastructure.Products.ValueObjects;
using MassTransit;
using CategoryName = ECommerce.Infrastructure.Categories.ValueObjects.Name;
using InventoryName = ECommerce.Infrastructure.Inventories.ValueObjects.Name;
using ProductName = ECommerce.Infrastructure.Products.ValueObjects.Name;

namespace ECommerce.Infrastructure.Data.Seed;
public static class InitialData
{
    public static List<Category> Categories { get; }
    public static List<Product> Products { get; }
    public static List<Inventory> Inventories { get; }
    public static List<InventoryItems> InventoryItems { get; }
    public static List<Customer> Customers { get; }
    static InitialData()
    {
        Categories =
        [
            Category.Create(CategoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c8")),
                CategoryName.Of("Food")),
            Category.Create(CategoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c9")),
                CategoryName.Of("Technology")),
        ];

        Inventories =
        [
            Inventory.Create(InventoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c4")),
                InventoryName.Of("Central-Inventory")),
            Inventory.Create(InventoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c5")),
                InventoryName.Of("Inventory-22")),
        ];

        Products =
        [
            Product.Create(ProductId.Of(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c0")),
                ProductName.Of("Cake"), Barcode.Of("1234567890"), true,
                CategoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c8")), Price.Of(50000), ProfitMargin.Of(0),
                Description.Of("It's a Cake")),
            Product.Create(ProductId.Of(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c1")),
                ProductName.Of("Pizza"), Barcode.Of("1234567891"), true,
                CategoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c8")), Price.Of(60000), ProfitMargin.Of(0),
                Description.Of("It's a Pizza")),
            Product.Create(ProductId.Of(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c2")),
                ProductName.Of("Drink"), Barcode.Of("1234567892"), true,
                CategoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c8")), Price.Of(70000), ProfitMargin.Of(0),
                Description.Of("It's a Drink")),
            Product.Create(ProductId.Of(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c3")),
                ProductName.Of("Keyboard"), Barcode.Of("1234567893"), true,
                CategoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c9")), Price.Of(80000), ProfitMargin.Of(0),
                Description.Of("It's a Keyboard")),
        ];

        InventoryItems =
        [
            ECommerce.Infrastructure.Inventories.Models.InventoryItems.AddProductToInventory(InventoryItemsId.Of(NewId.NextGuid()),
                InventoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c4")),
                ProductId.Of(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c0")), Quantity.Of(2)),
            ECommerce.Infrastructure.Inventories.Models.InventoryItems.AddProductToInventory(InventoryItemsId.Of(NewId.NextGuid()),
                InventoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c4")),
                ProductId.Of(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c1")), Quantity.Of(1)),
            ECommerce.Infrastructure.Inventories.Models.InventoryItems.AddProductToInventory(InventoryItemsId.Of(NewId.NextGuid()),
                InventoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c4")),
                ProductId.Of(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c2")), Quantity.Of(5)),
            ECommerce.Infrastructure.Inventories.Models.InventoryItems.AddProductToInventory(InventoryItemsId.Of(NewId.NextGuid()),
                InventoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c5")),
                ProductId.Of(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c3")), Quantity.Of(4)),
            Infrastructure.Inventories.Models.InventoryItems.AddProductToInventory(InventoryItemsId.Of(NewId.NextGuid()),
                InventoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c5")),
                ProductId.Of(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c3")), Quantity.Of(4), ProductStatus.Sold),
            Infrastructure.Inventories.Models.InventoryItems.AddProductToInventory(InventoryItemsId.Of(NewId.NextGuid()),
                InventoryId.Of(new Guid("3c5c0000-97c6-fc34-fc3c-08db322230c4")),
                ProductId.Of(new Guid("3c5c0000-97c6-fc34-fcd3-08db322230c1")), Quantity.Of(3), ProductStatus.Damaged),
        ];

        Customers =
        [
            Customer.Create(CustomerId.Of(new Guid("2c5c0000-97c6-fc34-fcd3-08db322230c0")), Infrastructure.Customers.ValueObjects.Name.Of("Admin"), Mobile.Of("09360000000"), Address.Of("Tehran", "Tehran", "Rey") ),
            Customer.Create(CustomerId.Of(new Guid("2c5c0000-97c6-fc34-fcd3-08db322230c1")), Infrastructure.Customers.ValueObjects.Name.Of("User"), Mobile.Of("09361111111"), Address.Of("Tehran", "Tehran", "Mirdamad"))
        ];
    }
}
