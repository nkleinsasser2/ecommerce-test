
using BuildingBlocks.EFCore;
using ECommerce.Infrastructure.Categories.Models;
using ECommerce.Infrastructure.Customers.Models;
using ECommerce.Infrastructure.Inventories.Models;
using ECommerce.Infrastructure.Orders.Models;
using ECommerce.Infrastructure.Products.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Data;
public sealed class ECommerceDbContext : AppDbContextBase
{
    public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
    {
    }
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<InventoryItems> InventoryItems => Set<InventoryItems>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        _ = builder.ApplyConfigurationsFromAssembly(typeof(InfrastructureRoot).Assembly);
        builder.FilterSoftDeletedProperties();
    }
}
