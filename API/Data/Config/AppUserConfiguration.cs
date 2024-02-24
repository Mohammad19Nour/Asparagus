using AsparagusN.Data.Entities.Identity;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsparagusN.Data.Config;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasOne(y => y.HomeAddress).WithMany().OnDelete(DeleteBehavior.NoAction);
        builder.HasMany(ur => ur.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

        builder.Property(s => s.Gender).HasConversion(o => o.ToString(),
            o => (Gender)Enum.Parse(typeof(Gender), o)
        );
    }
}