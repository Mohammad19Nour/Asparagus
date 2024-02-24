using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.Enums;
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

        builder.HasOne(x => x.DeliveryLocation).WithMany().HasForeignKey(y=>y.DeliveryLocationId).OnDelete(DeleteBehavior.NoAction);
        builder.HasMany(x => x.SelectedExtraOptions)
            .WithOne(y => y.UserPlanDay)
            .HasForeignKey(f => f.UserPlanDayId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(x => x.DeliveryPeriod)
            .HasConversion(o => o.ToString(), o => (Period)Enum.Parse(typeof(Period), o));
    }
}