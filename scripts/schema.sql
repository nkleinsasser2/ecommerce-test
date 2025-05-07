-- Create tables for the ecommerce database

-- Categories table
CREATE TABLE IF NOT EXISTS "Category" (
    "Id" UUID PRIMARY KEY,
    "Name" VARCHAR(255) NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL,
    "CreatedBy" INTEGER NOT NULL,
    "LastModified" TIMESTAMP NOT NULL,
    "LastModifiedBy" INTEGER NOT NULL,
    "Version" INTEGER NOT NULL,
    "IsDeleted" BOOLEAN NOT NULL DEFAULT false
);

-- Customers table
CREATE TABLE IF NOT EXISTS "Customer" (
    "Id" UUID PRIMARY KEY,
    "Name" VARCHAR(255) NOT NULL,
    "Mobile" VARCHAR(20) NOT NULL,
    "Address" TEXT NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL,
    "CreatedBy" INTEGER NOT NULL,
    "LastModified" TIMESTAMP NOT NULL,
    "LastModifiedBy" INTEGER NOT NULL,
    "Version" INTEGER NOT NULL,
    "IsDeleted" BOOLEAN NOT NULL DEFAULT false
);

-- Products table
CREATE TABLE IF NOT EXISTS "Product" (
    "Id" UUID PRIMARY KEY,
    "Name" VARCHAR(255) NOT NULL,
    "Barcode" VARCHAR(50) NOT NULL,
    "Description" TEXT,
    "CategoryId" UUID NOT NULL REFERENCES "Category"("Id"),
    "IsBreakable" BOOLEAN NOT NULL DEFAULT false,
    "Price" DECIMAL(10,2) NOT NULL,
    "NetPrice" DECIMAL(10,2) NOT NULL,
    "ProfitMargin" DECIMAL(10,2) NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL,
    "CreatedBy" INTEGER NOT NULL,
    "LastModified" TIMESTAMP NOT NULL,
    "LastModifiedBy" INTEGER NOT NULL,
    "Version" INTEGER NOT NULL,
    "IsDeleted" BOOLEAN NOT NULL DEFAULT false
);

-- Inventory table
CREATE TABLE IF NOT EXISTS "Inventory" (
    "Id" UUID PRIMARY KEY,
    "Name" VARCHAR(255) NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL,
    "CreatedBy" INTEGER NOT NULL,
    "LastModified" TIMESTAMP NOT NULL,
    "LastModifiedBy" INTEGER NOT NULL,
    "Version" INTEGER NOT NULL,
    "IsDeleted" BOOLEAN NOT NULL DEFAULT false
);

-- Inventory Items table
CREATE TABLE IF NOT EXISTS "InventoryItems" (
    "Id" UUID PRIMARY KEY,
    "InventoryId" UUID NOT NULL REFERENCES "Inventory"("Id"),
    "ProductId" UUID NOT NULL REFERENCES "Product"("Id"),
    "Quantity" INTEGER NOT NULL,
    "Status" VARCHAR(50) NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL,
    "CreatedBy" INTEGER NOT NULL,
    "LastModified" TIMESTAMP NOT NULL,
    "LastModifiedBy" INTEGER NOT NULL,
    "Version" INTEGER NOT NULL,
    "IsDeleted" BOOLEAN NOT NULL DEFAULT false
);

-- Orders table
CREATE TABLE IF NOT EXISTS "Order" (
    "Id" UUID PRIMARY KEY,
    "CustomerId" UUID NOT NULL REFERENCES "Customer"("Id"),
    "Status" VARCHAR(50) NOT NULL,
    "TotalPrice" DECIMAL(10,2) NOT NULL,
    "OrderDate" TIMESTAMP NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL,
    "CreatedBy" INTEGER NOT NULL,
    "LastModified" TIMESTAMP NOT NULL,
    "LastModifiedBy" INTEGER NOT NULL,
    "Version" INTEGER NOT NULL,
    "IsDeleted" BOOLEAN NOT NULL DEFAULT false
);

-- Order Items table
CREATE TABLE IF NOT EXISTS "OrderItem" (
    "Id" UUID PRIMARY KEY,
    "ProductId" UUID NOT NULL REFERENCES "Product"("Id"),
    "OrderId" UUID NOT NULL REFERENCES "Order"("Id"),
    "Quantity" INTEGER NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL,
    "CreatedBy" INTEGER NOT NULL,
    "LastModified" TIMESTAMP NOT NULL,
    "LastModifiedBy" INTEGER NOT NULL,
    "Version" INTEGER NOT NULL,
    "IsDeleted" BOOLEAN NOT NULL DEFAULT false
); 