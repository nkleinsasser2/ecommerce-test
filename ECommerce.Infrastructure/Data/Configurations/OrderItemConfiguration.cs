namespace ECommerce.Infrastructure.Data.Configurations;

using ECommerce.Infrastructure.Orders.Models;
using ECommerce.Infrastructure.Orders.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrderItemConfiguration: IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable(nameof(OrderItem));

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever()
            .HasConversion<Guid>(orderItemId => orderItemId.Value, dbId => OrderItemId.Of(dbId));

        builder.Property(r => r.IsDeleted);
        
        // Explicitly set required audit properties to NOT NULL
        builder.Property(r => r.CreatedAt).IsRequired();
        builder.Property(r => r.CreatedBy).IsRequired();
        builder.Property(r => r.LastModified).IsRequired();
        builder.Property(r => r.LastModifiedBy).IsRequired();

        builder.Property(r => r.Version).IsConcurrencyToken();

        builder.OwnsOne(
            x => x.Quantity,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(OrderItem.Quantity))
                    .IsRequired();
            }
        );

        builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId);
        
        // Configure the relationship with Order properly - ensure it is required and cascade delete
        builder.HasOne(x => x.Order)
            .WithMany() // This avoids navigation property collision with the _orderItems field
            .HasForeignKey(x => x.OrderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
