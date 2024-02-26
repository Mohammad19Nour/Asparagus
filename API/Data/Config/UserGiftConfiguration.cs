using AsparagusN.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsparagusN.Data.Config;

public class UserGiftConfiguration : IEntityTypeConfiguration<UserGift>
{
    public void Configure(EntityTypeBuilder<UserGift> builder)
    {
        builder.HasOne(x => x.User);
    }
}