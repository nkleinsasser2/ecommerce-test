
using ECommerce.Infrastructure.Inventories.Enums;
using ECommerce.Infrastructure.Inventories.Models;
using ECommerce.Infrastructure.Inventories.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations;
public class InventoryItemsConfigurations : IEntityTypeConfiguration<InventoryItems>
{
    public void Configure(EntityTypeBuilder<InventoryItems> builder)
    {
        _ = builder.ToTable(nameof(InventoryItems));

        _ = builder.HasKey(r => r.Id);
        _ = builder.Property(r => r.Id).ValueGeneratedNever()
            .HasConversion<Guid>(inventoryItemsId => inventoryItemsId.Value, dbId => InventoryItemsId.Of(dbId));


        _ = builder.Property(r => r.Version).IsConcurrencyToken();

        _ = builder.Property(x => x.Status)
            .IsRequired()
            .HasDefaultValue(ProductStatus.InStock)
            .HasConversion(
                x => x.ToString(),
                x => (ProductStatus)Enum.Parse(typeof(ProductStatus), x));

        _ = builder.OwnsOne(
            x => x.Quantity,
            a =>
            {
                _ = a.Property(p => p.Value)
                    .HasColumnName(nameof(InventoryItems.Quantity))
                    .HasMaxLength(20)
                    .IsRequired();
            }
        );

        _ = builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId);
        _ = builder.HasOne(x => x.Inventory).WithMany().HasForeignKey(x => x.InventoryId);
    }
}
