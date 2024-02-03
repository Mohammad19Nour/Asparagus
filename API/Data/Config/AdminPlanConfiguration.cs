using System.Globalization;
using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Entities.MealPlan;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AsparagusN.Data.Config;

public class AdminPlanConfiguration : IEntityTypeConfiguration<AdminPlan>
{
    public void Configure(EntityTypeBuilder<AdminPlan> builder)
    {
        builder.Property(x => x.AvailableDate).HasColumnType("date");
        builder.Property(s => s.PlanType).HasConversion(o => o.ToString(),
            o => (PlanType)Enum.Parse(typeof(PlanType), o));
        builder.HasMany(x=>x.Meals)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Meals)
            .WithOne(x => x.AdminPlan)
            .OnDelete(DeleteBehavior.Cascade);
    }
}