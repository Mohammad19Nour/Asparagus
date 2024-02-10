using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.Enums;

namespace AsparagusN.Entities.MealPlan;

public class UserPlanDay
{
    public int Id { get; set; }
    public int UserPlanId { get; set; }
    public UserPlan UserPlan { get; set; }
    public DateTime Day { get; set; }
    public ICollection<UserSelectedDrink> SelectedDrinks { get; set; }
    public ICollection<UserSelectedExtraOption> SelectedExtraOptions { get; set; }
    public ICollection<UserSelectedMeal> SelectedMeals { get; set; }
    public PlanOrderStatus Status { get; set; }
}