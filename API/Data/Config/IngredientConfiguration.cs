using AsparagusN.Data.Entities.Meal;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsparagusN.Data.Config;

public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.Property(x => x.TypeOfIngredient).HasConversion(i=>i.ToString()
        ,o=>(IngredientType)Enum.Parse(typeof(IngredientType),o));
        
    }
}