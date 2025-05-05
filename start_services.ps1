function Start-ServiceWithCheck {
    param (
        [string]$projectPath,
        [string]$serviceName
    )

    Write-Host "Starting $serviceName..."
    $process = Start-Process "dotnet" "run --project $projectPath" -PassThru
    Start-Sleep -Seconds 5

    if (Get-Process -Id $process.Id -ErrorAction SilentlyContinue) {
        Write-Host "$serviceName started successfully. PID: $($process.Id)"
    }
    else {
        Write-Host "Failed to start $serviceName."
    }
}

dotnet restore
dotnet build

Start-ServiceWithCheck ".\ECommerce.Categories.Api\ECommerce.Categories.Api.csproj" "ECommerce.Categories.Api"
Start-ServiceWithCheck ".\ECommerce.Orders.Api\ECommerce.Orders.Api.csproj" "ECommerce.Orders.Api"
Start-ServiceWithCheck ".\ECommerce.Products.Api\ECommerce.Products.Api.csproj" "ECommerce.Products.Api"
Start-ServiceWithCheck ".\ECommerce.Inventories.Api\ECommerce.Inventories.Api.csproj" "ECommerce.Inventories.Api"
Start-ServiceWithCheck ".\ECommerce.Infrastructure\ECommerce.Infrastructure.Api.csproj" "ECommerce.Infrastructure.Api"

Write-Host "Attempting to start all services."
