using AsparagusN.DTOs.MealDtos;
using AsparagusN.Entities;

namespace AsparagusN.DTOs.AdminPlanDtos;

public class AdminSelectedMealDto
{
    public int Id { get; set; }
    public MealWithoutIngredientsDto Meal { get; set; }
}