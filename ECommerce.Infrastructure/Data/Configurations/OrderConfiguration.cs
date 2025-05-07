namespace ECommerce.Infrastructure.Data.Configurations;

using ECommerce.Infrastructure.Orders.Enums;
using ECommerce.Infrastructure.Orders.Models;
using ECommerce.Infrastructure.Orders.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrderConfiguration: IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(nameof(Order));

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever()
            .HasConversion<Guid>(orderId => orderId.Value, dbId => OrderId.Of(dbId));

        builder.Property(r => r.Version).IsConcurrencyToken();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasDefaultValue(OrderStatus.Pending)
            .HasConversion(
                x => x.ToString(),
                x => (OrderStatus)Enum.Parse(typeof(OrderStatus), x));

        builder.OwnsOne(
            x => x.TotalPrice,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Order.TotalPrice))
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            }
        );

        builder.OwnsOne(
            x => x.OrderDate,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Order.OrderDate))
                    .IsRequired();
            }
        );

        builder.HasOne(x => x.Customer).WithMany().HasForeignKey(x => x.CustomerId);

        // Explicitly configure the one-to-many relationship with OrderItem
        // using the private backing field `_orderItems`.
        builder.HasMany<OrderItem>("_orderItems") // Use the exact name of the private field
            .WithOne(oi => oi.Order) // Specify the navigation property on OrderItem back to Order
            .HasForeignKey(oi => oi.OrderId); // Specify the foreign key on OrderItem
    }
}
