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
            o => (MealPlanType)Enum.Parse(typeof(MealPlanType), o));
        builder.HasMany(x=>x.Meals)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        
        var dateOnlyConverter = new ValueConverter<DateTime, string>(
            v => v.ToString("yyyy-MM-dd"),
            v => DateTime.ParseExact(v, "yyyy-MM-dd", CultureInfo.InvariantCulture)
        );
        
        
    }
}