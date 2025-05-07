#!/usr/bin/env pwsh

Write-Host "=== E-Commerce Microservices Test Runner ===" -ForegroundColor Cyan

# Stop any running containers
Write-Host "`nStopping any existing containers..." -ForegroundColor Cyan
docker-compose down

# Rebuild all containers
Write-Host "`nRebuilding all microservices..." -ForegroundColor Cyan
docker-compose build

# Start all containers
Write-Host "`nStarting all microservices..." -ForegroundColor Cyan
docker-compose up -d

# Allow services to start up (wait for PostgreSQL and API services)
Write-Host "`nWaiting for services to start up (10 seconds)..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

# Run the tests
Write-Host "`nRunning microservices tests..." -ForegroundColor Cyan
powershell.exe -ExecutionPolicy Bypass -File test-microservices.ps1

# Display logs if specified
$showLogs = $args[0] -eq "--show-logs"
if ($showLogs) {
    Write-Host "`nDisplaying container logs..." -ForegroundColor Cyan
    docker-compose logs
}

# Ask if user wants to keep containers running
$keepRunning = Read-Host "`nKeep containers running? (Y/N)"
if ($keepRunning -ne "Y") {
    Write-Host "`nStopping containers..." -ForegroundColor Cyan
    docker-compose down
}

Write-Host "`nTest run completed!" -ForegroundColor Green 