using AsparagusN.Data.Additions;
using AsparagusN.Data.Entities.MealPlan.Admin;

namespace AsparagusN.Entities.MealPlan;

public class DrinkItem
{
    public DrinkItem()
    {
    }

    public int AdminPlanId { get; set; }
    public AdminPlan AdminPlan { get; set; }
    public int DrinkId { get; set; }
    public Drink Drink { get; set; }
}