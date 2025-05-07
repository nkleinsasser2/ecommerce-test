namespace ECommerce.Infrastructure.Data.Seed;

using BuildingBlocks.EFCore;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class ECommerceDataSeeder : IDataSeeder
{
    private readonly ECommerceDbContext _eCommerceDbContext;
    private readonly ILogger<ECommerceDataSeeder> _logger;

    public ECommerceDataSeeder(ECommerceDbContext eCommerceDbContext, ILogger<ECommerceDataSeeder> logger)
    {
        _eCommerceDbContext = eCommerceDbContext;
        _logger = logger;
    }

    public async Task SeedAllAsync()
    {
        _logger.LogInformation("Starting database seeding...");
        await SeedCategoryAsync();
        await SeedInventoryAsync();
        await SeedProductAsync();
        await SeedInventoryItemsAsync();
        await SeedCustomerAsync();
        _logger.LogInformation("Database seeding completed.");
    }

    private async Task SeedCategoryAsync()
    {
        if (!await _eCommerceDbContext.Categories.AnyAsync())
        {
            await _eCommerceDbContext.Categories.AddRangeAsync(InitialData.Categories);
            await _eCommerceDbContext.SaveChangesAsync();
        }
    }

    private async Task SeedInventoryAsync()
    {
        _logger.LogInformation("Attempting to seed Inventories...");
        try
        {
            if (!await _eCommerceDbContext.Inventories.AnyAsync())
            {
                _logger.LogInformation("No existing inventories found. Adding initial data...");
                _logger.LogInformation("InitialData.Inventories count: {Count}", InitialData.Inventories.Count);
                await _eCommerceDbContext.Inventories.AddRangeAsync(InitialData.Inventories);
                int saved = await _eCommerceDbContext.SaveChangesAsync();
                _logger.LogInformation("Saved {Count} inventories to the database.", saved);
            }
            else
            {
                _logger.LogInformation("Inventories already exist in the database. Seeding skipped.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding inventories.");
            throw;
        }
    }

    private async Task SeedProductAsync()
    {
        if (!await _eCommerceDbContext.Products.AnyAsync())
        {
            await _eCommerceDbContext.Products.AddRangeAsync(InitialData.Products);
            await _eCommerceDbContext.SaveChangesAsync();
        }
    }

    private async Task SeedInventoryItemsAsync()
    {
        if (!await _eCommerceDbContext.InventoryItems.AnyAsync())
        {
            await _eCommerceDbContext.InventoryItems.AddRangeAsync(InitialData.InventoryItems);
            await _eCommerceDbContext.SaveChangesAsync();
        }
    }

    private async Task SeedCustomerAsync()
    {
        if (!await _eCommerceDbContext.Customers.AnyAsync())
        {
            await _eCommerceDbContext.Customers.AddRangeAsync(InitialData.Customers);
            await _eCommerceDbContext.SaveChangesAsync();
        }
    }
}
