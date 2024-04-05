using AsparagusN.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsparagusN.Data.Config;

public class FaqConfiguration:IEntityTypeConfiguration<FAQ>
{
    public void Configure(EntityTypeBuilder<FAQ> builder)
    {
        builder.HasMany(x => x.FAQChildern)
            .WithOne(c => c.ParentFAQ)
            .HasForeignKey(c => c.ParentFAQId);
        
        builder.HasOne(x => x.ParentFAQ)
            .WithMany(c => c.FAQChildern)
            .HasForeignKey(c => c.ParentFAQId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}