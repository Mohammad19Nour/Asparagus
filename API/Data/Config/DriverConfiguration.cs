using AsparagusN.Data.Entities;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsparagusN.Data.Config;

public class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        
        builder.Property(s => s.Period).HasConversion(o => o.ToString(),
            o => (Period)Enum.Parse(typeof(Period), o));

        builder.Property(s => s.Status).HasConversion(o => o.ToString(),
            o => (DriverStatus)Enum.Parse(typeof(DriverStatus), o));
    }
}