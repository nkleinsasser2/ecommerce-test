using ECommerce.Infrastructure.Categories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CategoryId = ECommerce.Infrastructure.Categories.ValueObjects.CategoryId;

namespace ECommerce.Infrastructure.Data.Configurations;
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        _ = builder.ToTable(nameof(Category));

        _ = builder.HasKey(r => r.Id);
        _ = builder.Property(r => r.Id).ValueGeneratedNever()
            .HasConversion<Guid>(categoryId => categoryId.Value, dbId => CategoryId.Of(dbId));

        _ = builder.Property(r => r.Version).IsConcurrencyToken();

        _ = builder.OwnsOne(
            x => x.Name,
            a =>
            {
                _ = a.Property(p => p.Value)
                    .HasColumnName(nameof(Category.Name))
                    .HasMaxLength(50)
                    .IsRequired();
            }
        );
    }
}
