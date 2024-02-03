using AsparagusN.Entities.MealPlan;
using AsparagusN.Enums;

namespace AsparagusN.Data.Entities.MealPlan.Admin;

public class AdminPlan
{
    public int Id { get; set; }
    public DateTime AvailableDate { get; set; }
    public PlanType PlanType { get; set; }
    public int Points { get; set; }
    public List<AdminSelectedMeal> Meals { get; set; } = new List<AdminSelectedMeal>();
}