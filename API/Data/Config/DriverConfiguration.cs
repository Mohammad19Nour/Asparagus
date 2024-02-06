using AsparagusN.Entities;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsparagusN.Data.Config;

public class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.HasOne(x => x.Zone)
            .WithMany(x => x.Drivers)
            .OnDelete(DeleteBehavior.Restrict);  
        builder.Property(s => s.Period).HasConversion(o=>o.ToString(),
            o=>(Period) Enum.Parse(typeof(Period),o)
        );
    }
}