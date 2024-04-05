using AsparagusN.Data.Entities.Meal;
using AsparagusN.Enums;

namespace AsparagusN.Data.Entities.MealPlan.AdminPlans;

public class AdminSelectedCarb
{
    public int Id { get; set; }
    public int CarbId { get; set; }
    public PlanTypeEnum PlanTypeEnum { get; set; }
    public Ingredient Carb { get; set; }
}