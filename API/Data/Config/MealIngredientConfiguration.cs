using AsparagusN.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsparagusN.Data.Config;

public class MealIngredientConfiguration : IEntityTypeConfiguration<MealIngredient>
{
    public void Configure(EntityTypeBuilder<MealIngredient> builder)
    {
        builder.HasKey(mi => new { mi.MealId,mi.IngredientId });
        builder.HasOne(mi => mi.Meal)
            .WithMany(m => m.Ingredients)
            .HasForeignKey(i=>i.MealId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}