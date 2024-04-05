using AsparagusN.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsparagusN.Data.Config;

public class GiftSelectionConfiguration : IEntityTypeConfiguration<GiftSelection>
{
    public void Configure(EntityTypeBuilder<GiftSelection> builder)
    {
        builder.HasOne(x => x.Meal).WithMany()
            .HasForeignKey(x=>x.MealId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}