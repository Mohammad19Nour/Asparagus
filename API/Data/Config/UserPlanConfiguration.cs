using AsparagusN.Data.Entities.MealPlan.UserPlan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsparagusN.Data.Config;

public class UserPlanConfiguration:IEntityTypeConfiguration<UserPlan>
{
    public void Configure(EntityTypeBuilder<UserPlan> builder)
    {
        builder.HasMany(x => x.Days).WithOne(y => y.UserPlan)
            .HasForeignKey(x => x.UserPlanId).OnDelete(DeleteBehavior.Cascade);
    }
}