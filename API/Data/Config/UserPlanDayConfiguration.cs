using AsparagusN.Data.Entities.MealPlan.UserPlan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsparagusN.Data.Config;

public class UserPlanDayConfiguration : IEntityTypeConfiguration<UserPlanDay>
{
    public void Configure(EntityTypeBuilder<UserPlanDay> builder)
    {
        builder.HasMany(x => x.SelectedMeals)
            .WithOne(y => y.UserPlanDay)
            .HasForeignKey(f => f.UserPlanDayId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.SelectedDrinks)
            .WithOne(y => y.UserPlanDay)
            .HasForeignKey(f => f.UserPlanDayId)
            .OnDelete(DeleteBehavior.Cascade);
       
        builder.HasMany(x => x.SelectedExtraOptions)
            .WithOne(y => y.UserPlanDay)
            .HasForeignKey(f => f.UserPlanDayId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}