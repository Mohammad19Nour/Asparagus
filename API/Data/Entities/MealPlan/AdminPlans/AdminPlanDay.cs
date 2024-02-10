using AsparagusN.Entities.MealPlan;
using AsparagusN.Enums;

namespace AsparagusN.Data.Entities.MealPlan.Admin;

public class AdminPlanDay
{
    public int Id { get; set; }
    public DateTime AvailableDate { get; set; }
    public PlanTypeEnum PlanType { get; set; }
    public List<AdminSelectedMeal> Meals { get; set; } = new List<AdminSelectedMeal>();
}