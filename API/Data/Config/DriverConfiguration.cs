using AsparagusN.Entities;
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
    }
}