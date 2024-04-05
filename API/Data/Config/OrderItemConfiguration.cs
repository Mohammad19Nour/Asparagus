using AsparagusN.Data.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsparagusN.Data.Config;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.OwnsOne(x => x.OrderedMeal,a=>a.WithOwner());
        builder.Property(i => i.Price).HasColumnType("decimal(18,2)");
    }
}