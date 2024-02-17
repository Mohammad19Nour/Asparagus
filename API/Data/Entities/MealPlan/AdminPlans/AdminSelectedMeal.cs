namespace AsparagusN.Data.Entities.MealPlan.AdminPlans;

public class AdminSelectedMeal
{
    public int Id { get; set; }
    public Meal.Meal Meal { get; set; }
    public int MealId { get; set; }
    public AdminPlanDay AdminPlanDay { get; set; }
    public int AdminPlanDayId { get; set; }
    
}