
using ECommerce.Infrastructure.Products.Models;
using ECommerce.Infrastructure.Products.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations;
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        _ = builder.ToTable(nameof(Product));

        _ = builder.HasKey(r => r.Id);
        _ = builder.Property(r => r.Id).ValueGeneratedNever()
            .HasConversion<Guid>(productId => productId.Value, dbId => ProductId.Of(dbId));


        _ = builder.Property(r => r.Version).IsConcurrencyToken();

        _ = builder.OwnsOne(
            x => x.Name,
            a =>
            {
                _ = a.Property(p => p.Value)
                    .HasColumnName(nameof(Product.Name))
                    .HasMaxLength(50)
                    .IsRequired();
            }
        );

        _ = builder.OwnsOne(
            x => x.Barcode,
            a =>
            {
                _ = a.Property(p => p.Value)
                    .HasColumnName(nameof(Product.Barcode))
                    .HasMaxLength(20)
                    .IsRequired();
            }
        );

        _ = builder.OwnsOne(
            x => x.Description,
            a =>
            {
                _ = a.Property(p => p.Value)
                    .HasColumnName(nameof(Product.Description))
                    .HasMaxLength(200)
                    .IsRequired();
            }
        );

        _ = builder.OwnsOne(
            x => x.Price,
            a =>
            {
                _ = a.Property(p => p.Value)
                    .HasColumnName(nameof(Product.Price))
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            }
        );

        _ = builder.OwnsOne(
            x => x.ProfitMargin,
            a =>
            {
                _ = a.Property(p => p.Value)
                    .HasColumnName(nameof(Product.ProfitMargin))
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            }
        );

        _ = builder.OwnsOne(
            x => x.NetPrice,
            a =>
            {
                _ = a.Property(p => p.Value)
                    .HasColumnName(nameof(Product.NetPrice))
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            }
        );

        _ = builder.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId);
    }
}
