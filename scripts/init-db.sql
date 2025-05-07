-- Create sample categories
INSERT INTO "Category" ("Id", "Name", "CreatedAt", "CreatedBy", "LastModified", "LastModifiedBy", "Version", "IsDeleted")
VALUES 
('3fa85f64-5717-4562-b3fc-2c963f66afa6', 'Electronics', NOW(), 1, NOW(), 1, 1, false),
('f47ac10b-58cc-4372-a567-0e02b2c3d479', 'Clothing', NOW(), 1, NOW(), 1, 1, false),
('b5a0b0e1-5b5f-4b0f-8b5a-0b0e1b5f4b0f', 'Home & Kitchen', NOW(), 1, NOW(), 1, 1, false);

-- Create sample customers
INSERT INTO "Customer" ("Id", "Name", "Mobile", "Address", "CreatedAt", "CreatedBy", "LastModified", "LastModifiedBy", "Version", "IsDeleted")
VALUES 
('a1b2c3d4-e5f6-4a5b-8c7d-9e0f1a2b3c4d', 'John Smith', '1234567890', '123 Main St, Anytown, USA', NOW(), 1, NOW(), 1, 1, false),
('b2c3d4e5-f6a7-5b6c-9d0e-1f2a3b4c5d6e', 'Jane Doe', '0987654321', '456 Elm St, Anytown, USA', NOW(), 1, NOW(), 1, 1, false),
('c3d4e5f6-a7b8-6c7d-0e1f-2a3b4c5d6e7f', 'Bob Johnson', '5551234567', '789 Oak St, Anytown, USA', NOW(), 1, NOW(), 1, 1, false);

-- Create sample products
INSERT INTO "Product" ("Id", "Name", "Barcode", "Description", "CategoryId", "IsBreakable", "Price", "NetPrice", "ProfitMargin", "CreatedAt", "CreatedBy", "LastModified", "LastModifiedBy", "Version", "IsDeleted")
VALUES 
('d4e5f6a7-b8c9-7d0e-1f2a-3b4c5d6e7f8a', 'Smartphone X', 'SM-X-1234', 'Latest smartphone with advanced features', '3fa85f64-5717-4562-b3fc-2c963f66afa6', true, 999.99, 799.99, 200.00, NOW(), 1, NOW(), 1, 1, false),
('e5f6a7b8-c9d0-8e1f-2a3b-4c5d6e7f8a9b', 'T-shirt Basic', 'TS-B-5678', 'Comfortable cotton t-shirt', 'f47ac10b-58cc-4372-a567-0e02b2c3d479', false, 19.99, 9.99, 10.00, NOW(), 1, NOW(), 1, 1, false),
('f6a7b8c9-d0e1-9f2a-3b4c-5d6e7f8a9b0c', 'Coffee Maker', 'CM-PRO-9012', 'Professional coffee maker for home use', 'b5a0b0e1-5b5f-4b0f-8b5a-0b0e1b5f4b0f', true, 129.99, 79.99, 50.00, NOW(), 1, NOW(), 1, 1, false),
('a7b8c9d0-e1f2-0a3b-4c5d-6e7f8a9b0c1d', 'Laptop Pro', 'LP-PRO-3456', 'High performance laptop for professionals', '3fa85f64-5717-4562-b3fc-2c963f66afa6', true, 1499.99, 1199.99, 300.00, NOW(), 1, NOW(), 1, 1, false),
('b8c9d0e1-f2a3-1b4c-5d6e-7f8a9b0c1d2e', 'Jeans Classic', 'JC-DEM-7890', 'Classic denim jeans', 'f47ac10b-58cc-4372-a567-0e02b2c3d479', false, 49.99, 29.99, 20.00, NOW(), 1, NOW(), 1, 1, false);

-- Create sample inventory
INSERT INTO "Inventory" ("Id", "Name", "CreatedAt", "CreatedBy", "LastModified", "LastModifiedBy", "Version", "IsDeleted")
VALUES 
('c9d0e1f2-a3b4-2c5d-6e7f-8a9b0c1d2e3f', 'Main Warehouse', NOW(), 1, NOW(), 1, 1, false),
('d0e1f2a3-b4c5-3d6e-7f8a-9b0c1d2e3f4a', 'Store Front', NOW(), 1, NOW(), 1, 1, false);

-- Create sample inventory items
INSERT INTO "InventoryItems" ("Id", "InventoryId", "ProductId", "Quantity", "Status", "CreatedAt", "CreatedBy", "LastModified", "LastModifiedBy", "Version", "IsDeleted")
VALUES 
('e1f2a3b4-c5d6-4e7f-8a9b-0c1d2e3f4a5b', 'c9d0e1f2-a3b4-2c5d-6e7f-8a9b0c1d2e3f', 'd4e5f6a7-b8c9-7d0e-1f2a-3b4c5d6e7f8a', 50, 'InStock', NOW(), 1, NOW(), 1, 1, false),
('f2a3b4c5-d6e7-5f8a-9b0c-1d2e3f4a5b6c', 'c9d0e1f2-a3b4-2c5d-6e7f-8a9b0c1d2e3f', 'e5f6a7b8-c9d0-8e1f-2a3b-4c5d6e7f8a9b', 200, 'InStock', NOW(), 1, NOW(), 1, 1, false),
('a3b4c5d6-e7f8-6a9b-0c1d-2e3f4a5b6c7d', 'c9d0e1f2-a3b4-2c5d-6e7f-8a9b0c1d2e3f', 'f6a7b8c9-d0e1-9f2a-3b4c-5d6e7f8a9b0c', 30, 'InStock', NOW(), 1, NOW(), 1, 1, false),
('b4c5d6e7-f8a9-7b0c-1d2e-3f4a5b6c7d8e', 'd0e1f2a3-b4c5-3d6e-7f8a-9b0c1d2e3f4a', 'd4e5f6a7-b8c9-7d0e-1f2a-3b4c5d6e7f8a', 15, 'InStock', NOW(), 1, NOW(), 1, 1, false),
('c5d6e7f8-a9b0-8c1d-2e3f-4a5b6c7d8e9f', 'd0e1f2a3-b4c5-3d6e-7f8a-9b0c1d2e3f4a', 'b8c9d0e1-f2a3-1b4c-5d6e-7f8a9b0c1d2e', 100, 'InStock', NOW(), 1, NOW(), 1, 1, false);

-- Create sample orders
INSERT INTO "Order" ("Id", "CustomerId", "Status", "TotalPrice", "OrderDate", "CreatedAt", "CreatedBy", "LastModified", "LastModifiedBy", "Version", "IsDeleted")
VALUES 
('d6e7f8a9-b0c1-9d2e-3f4a-5b6c7d8e9f0a', 'a1b2c3d4-e5f6-4a5b-8c7d-9e0f1a2b3c4d', 'Completed', 1019.98, NOW(), NOW(), 1, NOW(), 1, 1, false),
('e7f8a9b0-c1d2-0e3f-4a5b-6c7d8e9f0a1b', 'b2c3d4e5-f6a7-5b6c-9d0e-1f2a3b4c5d6e', 'Processing', 179.97, NOW(), NOW(), 1, NOW(), 1, 1, false),
('f8a9b0c1-d2e3-1f4a-5b6c-7d8e9f0a1b2c', 'c3d4e5f6-a7b8-6c7d-0e1f-2a3b4c5d6e7f', 'Pending', 129.99, NOW(), NOW(), 1, NOW(), 1, 1, false);

-- Create sample order items
INSERT INTO "OrderItem" ("Id", "ProductId", "OrderId", "Quantity", "CreatedAt", "CreatedBy", "LastModified", "LastModifiedBy", "Version", "IsDeleted")
VALUES 
('a9b0c1d2-e3f4-2a5b-6c7d-8e9f0a1b2c3d', 'd4e5f6a7-b8c9-7d0e-1f2a-3b4c5d6e7f8a', 'd6e7f8a9-b0c1-9d2e-3f4a-5b6c7d8e9f0a', 1, NOW(), 1, NOW(), 1, 1, false),
('b0c1d2e3-f4a5-3b6c-7d8e-9f0a1b2c3d4e', 'e5f6a7b8-c9d0-8e1f-2a3b-4c5d6e7f8a9b', 'd6e7f8a9-b0c1-9d2e-3f4a-5b6c7d8e9f0a', 1, NOW(), 1, NOW(), 1, 1, false),
('c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f', 'b8c9d0e1-f2a3-1b4c-5d6e-7f8a9b0c1d2e', 'e7f8a9b0-c1d2-0e3f-4a5b-6c7d8e9f0a1b', 3, NOW(), 1, NOW(), 1, 1, false),
('d2e3f4a5-b6c7-5d8e-9f0a-1b2c3d4e5f6a', 'f6a7b8c9-d0e1-9f2a-3b4c-5d6e7f8a9b0c', 'f8a9b0c1-d2e3-1f4a-5b6c-7d8e9f0a1b2c', 1, NOW(), 1, NOW(), 1, 1, false); 