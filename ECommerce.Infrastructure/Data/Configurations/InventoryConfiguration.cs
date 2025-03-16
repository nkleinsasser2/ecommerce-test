namespace ECommerce.Infrastructure.Data.Configurations;

using ECommerce.Infrastructure.Inventories.Models;
using ECommerce.Infrastructure.Inventories.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        _ = builder.ToTable(nameof(Inventory));

        _ = builder.HasKey(r => r.Id);
        _ = builder.Property(r => r.Id).ValueGeneratedNever()
            .HasConversion<Guid>(inventoryId => inventoryId.Value, dbId => InventoryId.Of(dbId));

        _ = builder.Property(r => r.Version).IsConcurrencyToken();

        _ = builder.OwnsOne(
            x => x.Name,
            a =>
            {
                _ = a.Property(p => p.Value)
                    .HasColumnName(nameof(Inventory.Name))
                    .HasMaxLength(50)
                    .IsRequired();
            }
        );
    }
}

