using System;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsOne(x => x.ShippingAddress, o => o.WithOwner());        // order owns the shipping address
        builder.OwnsOne(x => x.PaymentSummary, o => o.WithOwner());
        builder.Property(x => x.Status).HasConversion(
            o => o.ToString(),
            o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o)
        );
        builder.Property(x => x.Subtotal).HasColumnType("decimal(18,2)");
        builder.HasMany(x => x.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);      // when we delete order, we also delete order items in that order
        builder.Property(x => x.OrderDate).HasConversion(
            d => d.ToUniversalTime(),
            d=> DateTime.SpecifyKind(d, DateTimeKind.Utc)
        );
    }
}
