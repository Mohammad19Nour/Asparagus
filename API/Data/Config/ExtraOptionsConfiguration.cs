using AsparagusN.Data.Entities;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsparagusN.Data.Config;

public class ExtraOptionsConfiguration : IEntityTypeConfiguration<ExtraOption>
{
    public void Configure(EntityTypeBuilder<ExtraOption> builder)
    {
        builder.Property(x => x.OptionType).HasConversion(x => x.ToString(),
            o=>(ExtraOptionType)Enum.Parse(typeof(ExtraOptionType),o));
    }
}