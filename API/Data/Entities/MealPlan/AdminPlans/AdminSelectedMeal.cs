using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Entities;

namespace AsparagusN.Data.Entities.MealPlan.AdminPlans;

public class AdminSelectedMeal
{
    public int Id { get; set; }
    public Meal Meal { get; set; }
    public int MealId { get; set; }
    public AdminPlanDay AdminPlanDay { get; set; }
    public int AdminPlanDayId { get; set; }
    
}