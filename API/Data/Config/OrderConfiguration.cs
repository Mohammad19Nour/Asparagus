using AsparagusN.Entities.OrderAggregate;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsparagusN.Data.Config;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
            builder.Property(s => s.Status).HasConversion(o=>o.ToString(),
            o=>(OrderStatus) Enum.Parse(typeof(OrderStatus),o)
            );
        builder.HasMany(o => o.Items).WithOne().OnDelete(DeleteBehavior.Cascade);
    }
}