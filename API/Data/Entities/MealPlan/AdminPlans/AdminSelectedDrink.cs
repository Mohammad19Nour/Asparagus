using AsparagusN.Enums;

namespace AsparagusN.Data.Entities.MealPlan.AdminPlans;

public class AdminSelectedDrink
{
    public int Id { get; set; }
    public int DrinkId { get; set; }
    public PlanTypeEnum PlanTypeEnum { get; set; }
    public Drink Drink { get; set; }
}