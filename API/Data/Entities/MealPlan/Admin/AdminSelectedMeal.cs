using AsparagusN.Data.Entities.MealPlan.Admin;

namespace AsparagusN.Entities.MealPlan;

public class AdminSelectedMeal
{
    public Meal Meal { get; set; }
    public int MealId { get; set; }
    public AdminPlan AdminPlan { get; set; }
    public int AdminPlanId { get; set; }
}