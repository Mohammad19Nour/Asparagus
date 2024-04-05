using AsparagusN.DTOs.IngredientDtos;

namespace AsparagusN.DTOs.MealDtos;

public class MealIngredientDetailsDto
{
    public decimal Weight { get; set; }
    public IngredientDto Ingredient { get; set; }
}