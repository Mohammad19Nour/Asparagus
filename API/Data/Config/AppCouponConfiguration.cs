using AsparagusN.Data.Entities;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsparagusN.Data.Config;

public class AppCouponConfiguration : IEntityTypeConfiguration<AppCoupon>
{
    public void Configure(EntityTypeBuilder<AppCoupon> builder)
    {
        builder.Property(x => x.Type)
            .HasConversion(o => o.ToString(), b => (AppCouponType)Enum.Parse(typeof(AppCouponType), b));
    }
}