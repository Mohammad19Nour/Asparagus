using AsparagusN.DTOs.MealDtos;

namespace AsparagusN.DTOs.AdminPlanDtos;

public class AdminSelectedMealDto
{
    public int Id { get; set; }
    public MealWithoutIngredientsDto Meal { get; set; }
}