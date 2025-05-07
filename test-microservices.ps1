$ErrorActionPreference = "Continue"
$StartTime = Get-Date

$productsApi = "http://localhost:5001/api/v1/catalog"
$categoriesApi = "http://localhost:5002/api/v1/catalog"
$ordersApi = "http://localhost:5003/api"
$inventoriesApiBaseCatalog = "http://localhost:5004/api/v1/catalog"
$inventoriesApiBaseInventory = "http://localhost:5004/api/v1/inventory"

$colorSuccess = "Green"
$colorWarning = "Yellow"
$colorError = "Red"
$colorInfo = "Cyan"
$colorHeader = "Magenta"

Write-Host "`n===========================================================" -ForegroundColor $colorHeader
Write-Host "MICROSERVICE ARCHITECTURE TESTING SUITE" -ForegroundColor $colorHeader
Write-Host "===========================================================`n" -ForegroundColor $colorHeader

function Write-TestHeader($name) {
    Write-Host "`n----------------------------------------------------------" -ForegroundColor $colorHeader
    Write-Host "TEST: $name" -ForegroundColor $colorHeader
    Write-Host "----------------------------------------------------------" -ForegroundColor $colorHeader
}

function Write-TestResult($name, $success, $data, $errorMessage = $null) {
    if ($success) {
        Write-Host "✅ $name - SUCCESS" -ForegroundColor $colorSuccess
        if ($data) {
            Write-Host "   Data: $($data | ConvertTo-Json -Depth 4 -Compress)" -ForegroundColor $colorInfo
        }
    } else {
        Write-Host "❌ $name - FAILED" -ForegroundColor $colorError
        if ($errorMessage) {
            Write-Host "   Error: $errorMessage" -ForegroundColor $colorError
        }
    }
}

function Test-ApiAvailability($name, $url) {
    Write-TestHeader "Testing $name availability (via Swagger UI)"
    $uri = [System.Uri]$url
    $baseUrl = $uri.GetComponents([System.UriComponents]::SchemeAndServer, [System.UriFormat]::Unescaped)
    $swaggerUrl = $baseUrl + "/swagger/index.html"
    try {
        Write-Host "   Checking: $swaggerUrl" -ForegroundColor Gray
        $response = Invoke-WebRequest -Uri $swaggerUrl -Method GET -UseBasicParsing -ErrorAction Stop # Stop on error
        if ($response.StatusCode -eq 200) {
            Write-TestResult "$name Base Check" $true
            return $true
        } else {
            Write-TestResult "$name Base Check" $false $null "Status code: $($response.StatusCode)"
            return $false
        }
    } catch {
        Write-Host "   $name appears to be unavailable. Error: $($_.Exception.Message)" -ForegroundColor $colorError
        Write-TestResult "$name Base Check" $false
        return $false
    }
}

function Invoke-ApiRequest($method, $url, $body = $null) {
    $headers = @{
        'Content-Type' = 'application/json'
        'Accept' = 'application/json'
    }
    
    try {
        if ($body) {
            $bodyJson = ConvertTo-Json -InputObject $body -Depth 4
            $response = Invoke-RestMethod -Method $method -Uri $url -Headers $headers -Body $bodyJson
        } else {
            $response = Invoke-RestMethod -Method $method -Uri $url -Headers $headers
        }
        return @{
            Success = $true
            Data = $response
            Error = $null
        }
    } catch {
        return @{
            Success = $false
            Data = $null
            Error = $_.Exception.Message
        }
    }
}

Write-Host "`n==== PRODUCTS API TESTS ====" -ForegroundColor $colorHeader

Test-ApiAvailability "Products API" $productsApi

Write-TestHeader "Retrieving all products"
$getProductsResult = Invoke-ApiRequest "GET" "$productsApi/get-products-by-page?PageNumber=1&PageSize=10&Filters=&SortOrder="
Write-TestResult "Get All Products" $getProductsResult.Success $getProductsResult.Data $getProductsResult.Error

Write-TestHeader "Creating a new product"
$newProduct = @{
    name = "Test Product $(Get-Random)"
    barcode = "TEST-$(Get-Random -Minimum 1000 -Maximum 9999)"
    weighted = $false
    categoryId = "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    price = 25000.00 
    profitMargin = 0.3
    description = "Test product created during API testing"
}
$createProductResult = Invoke-ApiRequest "POST" "$productsApi/product" $newProduct
Write-TestResult "Create Product" $createProductResult.Success $createProductResult.Data $createProductResult.Error

if ($createProductResult.Success) {
    $productId = $createProductResult.Data.id
    Write-Host "   Created product with ID: $productId" -ForegroundColor $colorInfo
}

Write-Host "`n==== CATEGORIES API TESTS ====" -ForegroundColor $colorHeader

Test-ApiAvailability "Categories API" $categoriesApi

Write-TestHeader "Retrieving all categories"
$getCategoriesResult = Invoke-ApiRequest "GET" "$categoriesApi/get-categories-by-page?PageNumber=1&PageSize=10&Filters=&SortOrder="
Write-TestResult "Get All Categories" $getCategoriesResult.Success $getCategoriesResult.Data $getCategoriesResult.Error

Write-TestHeader "Creating a new category"
$newCategory = @{
    name = "Test Category $(Get-Random)"
}
$createCategoryResult = Invoke-ApiRequest "POST" "$categoriesApi/category" $newCategory
Write-TestResult "Create Category" $createCategoryResult.Success $createCategoryResult.Data $createCategoryResult.Error

if ($createCategoryResult.Success) {
    $categoryId = $createCategoryResult.Data.id
    Write-Host "   Created category with ID: $categoryId" -ForegroundColor $colorInfo
}

Write-Host "`n==== INVENTORIES API TESTS ====" -ForegroundColor $colorHeader

Test-ApiAvailability "Inventories API" $inventoriesApiBaseCatalog

Write-TestHeader "Retrieving all inventory items"
$getInventoriesResult = Invoke-ApiRequest "GET" "$inventoriesApiBaseCatalog/get-all-inventory-items-by-page?PageNumber=1&PageSize=10&Filters=&SortOrder="
Write-TestResult "Get All Inventory Items" $getInventoriesResult.Success $getInventoriesResult.Data $getInventoriesResult.Error

$inventoryId = "c9d0e1f2-a3b4-2c5d-6e7f-8a9b0c1d2e3f" # Use existing seed inventory ID
Write-Host "   Using existing inventory with ID: $inventoryId" -ForegroundColor $colorInfo

if ($productId) {
    Write-TestHeader "Adding product to inventory"
    $newInventoryItem = @{
        inventoryId = $inventoryId
        productId = $productId
        quantity = 100
    }
    $addItemResult = Invoke-ApiRequest "POST" "$inventoriesApiBaseInventory/add-product-to-inventory" $newInventoryItem
    Write-TestResult "Add Inventory Item" $addItemResult.Success $addItemResult.Data $addItemResult.Error
} else {
    Write-Host "   Skipping inventory item creation because no product ID is available" -ForegroundColor $colorWarning
}

Write-Host "`n==== ORDERS API TESTS ====" -ForegroundColor $colorHeader

$ordersApiBaseUrl = $ordersApi
Test-ApiAvailability "Orders API" $ordersApiBaseUrl 

Write-TestHeader "Creating a new order"
$createOrderUrl = "$ordersApi/order/register-new-order"
Write-Host "   Target URL: $createOrderUrl" -ForegroundColor Gray

$newOrder = @{
    customerId = "a1b2c3d4-e5f6-4a5b-8c7d-9e0f1a2b3c4d"
    items = @(
        @{
            productId = if ($productId) { $productId } else { "d4e5f6a7-b8c9-7d0e-1f2a-3b4c5d6e7f8a" }
            quantity = 3
        }
    )
    discountType = 0
    discountValue = 0
}
$createOrderResult = Invoke-ApiRequest "POST" $createOrderUrl $newOrder
Write-TestResult "Create Order" $createOrderResult.Success $createOrderResult.Data $createOrderResult.Error

Write-Host "`n===========================================================`n" -ForegroundColor $colorHeader
$duration = (Get-Date) - $StartTime
Write-Host "Test Duration: $($duration.Minutes) minutes and $($duration.Seconds) seconds" -ForegroundColor $colorHeader
Write-Host "`n===========================================================`n" -ForegroundColor $colorHeader 