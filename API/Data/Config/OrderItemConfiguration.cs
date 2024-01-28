using AsparagusN.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsparagusN.Data.Config;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.OwnsOne(i=>i.OrderedMeal, mo =>
        {   
            mo.WithOwner();
        });
        builder.Property(i => i.Price).HasColumnType("decimal(18,2)");
    }
}