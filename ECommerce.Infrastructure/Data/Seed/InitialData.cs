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
            Category.Create(CategoryId.Of(new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6")),
                CategoryName.Of("Electronics")),
            Category.Create(CategoryId.Of(new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479")),
                CategoryName.Of("Clothing")),
            Category.Create(CategoryId.Of(new Guid("b5a0b0e1-5b5f-4b0f-8b5a-0b0e1b5f4b0f")),
                CategoryName.Of("Home & Kitchen")),
        ];

        Inventories =
        [
            Inventory.Create(InventoryId.Of(new Guid("c9d0e1f2-a3b4-2c5d-6e7f-8a9b0c1d2e3f")),
                InventoryName.Of("Main Warehouse")),
            Inventory.Create(InventoryId.Of(new Guid("d0e1f2a3-b4c5-3d6e-7f8a-9b0c1d2e3f4a")),
                InventoryName.Of("Store Front")),
        ];

        Products =
        [
            Product.Create(ProductId.Of(new Guid("d4e5f6a7-b8c9-7d0e-1f2a-3b4c5d6e7f8a")),
                ProductName.Of("Smartphone X"), Barcode.Of("SM-X-1234"), true,
                CategoryId.Of(new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6")), Price.Of(999.99m), ProfitMargin.Of(200.00m),
                Description.Of("Latest smartphone with advanced features")),
            Product.Create(ProductId.Of(new Guid("e5f6a7b8-c9d0-8e1f-2a3b-4c5d6e7f8a9b")),
                ProductName.Of("T-shirt Basic"), Barcode.Of("TS-B-5678"), false,
                CategoryId.Of(new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479")), Price.Of(19.99m), ProfitMargin.Of(10.00m),
                Description.Of("Comfortable cotton t-shirt")),
            Product.Create(ProductId.Of(new Guid("f6a7b8c9-d0e1-9f2a-3b4c-5d6e7f8a9b0c")),
                ProductName.Of("Coffee Maker"), Barcode.Of("CM-PRO-9012"), true,
                CategoryId.Of(new Guid("b5a0b0e1-5b5f-4b0f-8b5a-0b0e1b5f4b0f")), Price.Of(129.99m), ProfitMargin.Of(50.00m),
                Description.Of("Professional coffee maker for home use")),
            Product.Create(ProductId.Of(new Guid("a7b8c9d0-e1f2-0a3b-4c5d-6e7f8a9b0c1d")),
                ProductName.Of("Laptop Pro"), Barcode.Of("LP-PRO-3456"), true,
                CategoryId.Of(new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6")), Price.Of(1499.99m), ProfitMargin.Of(300.00m),
                Description.Of("High performance laptop for professionals")),
            Product.Create(ProductId.Of(new Guid("b8c9d0e1-f2a3-1b4c-5d6e-7f8a9b0c1d2e")),
                ProductName.Of("Jeans Classic"), Barcode.Of("JC-DEM-7890"), false,
                CategoryId.Of(new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479")), Price.Of(49.99m), ProfitMargin.Of(20.00m),
                Description.Of("Classic denim jeans")),
        ];

        InventoryItems =
        [
            ECommerce.Infrastructure.Inventories.Models.InventoryItems.AddProductToInventory(InventoryItemsId.Of(NewId.NextGuid()),
                InventoryId.Of(new Guid("c9d0e1f2-a3b4-2c5d-6e7f-8a9b0c1d2e3f")), // Main Warehouse
                ProductId.Of(new Guid("d4e5f6a7-b8c9-7d0e-1f2a-3b4c5d6e7f8a")), Quantity.Of(50)), // Smartphone X
            ECommerce.Infrastructure.Inventories.Models.InventoryItems.AddProductToInventory(InventoryItemsId.Of(NewId.NextGuid()),
                InventoryId.Of(new Guid("c9d0e1f2-a3b4-2c5d-6e7f-8a9b0c1d2e3f")), // Main Warehouse
                ProductId.Of(new Guid("e5f6a7b8-c9d0-8e1f-2a3b-4c5d6e7f8a9b")), Quantity.Of(200)), // T-shirt Basic
            ECommerce.Infrastructure.Inventories.Models.InventoryItems.AddProductToInventory(InventoryItemsId.Of(NewId.NextGuid()),
                InventoryId.Of(new Guid("c9d0e1f2-a3b4-2c5d-6e7f-8a9b0c1d2e3f")), // Main Warehouse
                ProductId.Of(new Guid("f6a7b8c9-d0e1-9f2a-3b4c-5d6e7f8a9b0c")), Quantity.Of(30)), // Coffee Maker
            ECommerce.Infrastructure.Inventories.Models.InventoryItems.AddProductToInventory(InventoryItemsId.Of(NewId.NextGuid()),
                InventoryId.Of(new Guid("d0e1f2a3-b4c5-3d6e-7f8a-9b0c1d2e3f4a")), // Store Front
                ProductId.Of(new Guid("d4e5f6a7-b8c9-7d0e-1f2a-3b4c5d6e7f8a")), Quantity.Of(15)), // Smartphone X
            ECommerce.Infrastructure.Inventories.Models.InventoryItems.AddProductToInventory(InventoryItemsId.Of(NewId.NextGuid()),
                InventoryId.Of(new Guid("d0e1f2a3-b4c5-3d6e-7f8a-9b0c1d2e3f4a")), // Store Front
                ProductId.Of(new Guid("b8c9d0e1-f2a3-1b4c-5d6e-7f8a9b0c1d2e")), Quantity.Of(100)), // Jeans Classic
        ];

        Customers =
        [
            Customer.Create(CustomerId.Of(new Guid("a1b2c3d4-e5f6-4a5b-8c7d-9e0f1a2b3c4d")),
                Infrastructure.Customers.ValueObjects.Name.Of("John Smith"), Mobile.Of("1234567890"), Address.Of("Anytown", "USA", "123 Main St")),
            Customer.Create(CustomerId.Of(new Guid("b2c3d4e5-f6a7-5b6c-9d0e-1f2a3b4c5d6e")),
                Infrastructure.Customers.ValueObjects.Name.Of("Jane Doe"), Mobile.Of("9876543210"), Address.Of("Anytown", "USA", "456 Elm St")),
            Customer.Create(CustomerId.Of(new Guid("c3d4e5f6-a7b8-6c7d-0e1f-2a3b4c5d6e7f")),
                Infrastructure.Customers.ValueObjects.Name.Of("Bob Johnson"), Mobile.Of("5551234567"), Address.Of("Anytown", "USA", "789 Oak St")),
        ];
    }
}
