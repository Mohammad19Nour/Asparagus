using AsparagusN.Data.Entities;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsparagusN.Data.Config;

public class CashierConfiguration:IEntityTypeConfiguration<Cashier>
{
    public void Configure(EntityTypeBuilder<Cashier> builder)
    {
        builder.Property(s => s.Period).HasConversion(o=>o.ToString(),
            o=>(Period) Enum.Parse(typeof(Period),o)
        );
    }
}