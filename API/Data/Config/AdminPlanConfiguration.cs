using System.Globalization;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AsparagusN.Data.Config;

public class AdminPlanConfiguration : IEntityTypeConfiguration<AdminPlanDay>
{
    public void Configure(EntityTypeBuilder<AdminPlanDay> builder)
    {
        builder.Property(x => x.AvailableDate).HasColumnType("date");
        builder.Property(s => s.PlanType).HasConversion(o => o.ToString(),
            o => (PlanTypeEnum)Enum.Parse(typeof(PlanTypeEnum), o));
        builder.HasMany(x=>x.Meals)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Meals)
            .WithOne(x => x.AdminPlanDay)
            .OnDelete(DeleteBehavior.Cascade);
    }
}