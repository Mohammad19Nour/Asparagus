using AsparagusN.Data.Entities.Meal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsparagusN.Data.Config;

public class MealAllergyConfiguration : IEntityTypeConfiguration<MealAllergy>
{
    public void Configure(EntityTypeBuilder<MealAllergy> builder)
    {
        builder.HasKey(al => new {
            al.MealId,al.AllergyId
        });

        builder.HasOne(m => m.Meal)
            .WithMany(ma => ma.Allergies)
            .HasForeignKey(ma => ma.MealId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}