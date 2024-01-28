using AsparagusN.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsparagusN.Data.Config;

public class MealConfiguration : IEntityTypeConfiguration<Meal>
{
    public void Configure(EntityTypeBuilder<Meal> builder)
    {
        builder.HasMany(i => i.Ingredients).WithOne().OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(i => i.Allergies).WithOne().OnDelete(DeleteBehavior.Cascade);
        builder.Property(i => i.Price).HasColumnType("decimal(18,2)");
        builder.HasOne(i => i.Category).WithMany().HasForeignKey(c => c.CategoryId);
    }
}