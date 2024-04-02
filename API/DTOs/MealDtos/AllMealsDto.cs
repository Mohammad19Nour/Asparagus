namespace AsparagusN.DTOs.MealDtos;

public class AllMealsDto : MealWithIngredientsDto
{
    public bool IsMainMenu { get; set; }
    public bool IsMealPlan { get; set; }
}