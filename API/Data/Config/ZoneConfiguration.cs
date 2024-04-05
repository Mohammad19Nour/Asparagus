using AsparagusN.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsparagusN.Data.Config;

public class ZoneConfiguration : IEntityTypeConfiguration<Zone>
{
    public void Configure(EntityTypeBuilder<Zone> builder)
    {
        builder.HasMany(z => z.Drivers).WithOne(t => t.Zone)
            .HasForeignKey(f => f.ZoneId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}