using AsparagusN.Data.Entities.MealPlan.UserPlan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsparagusN.Data.Config;

public class UserSelectedMealConfiguration : IEntityTypeConfiguration<UserSelectedMeal>
{
    public void Configure(EntityTypeBuilder<UserSelectedMeal> builder)
    {
        builder.HasOne(x => x.ChangedCarb)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}