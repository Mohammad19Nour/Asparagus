﻿using AsparagusN.Entities.MealPlan;
using AsparagusN.Enums;

namespace AsparagusN.Data.Entities.MealPlan.UserPlan;

public class UserPlanDay
{
    public int Id { get; set; }
    public int UserPlanId { get; set; }
    public UserPlan UserPlan { get; set; }
    public DateTime Day { get; set; }
    public ICollection<UserSelectedDrink> SelectedDrinks { get; set; } = new List<UserSelectedDrink>();

    public ICollection<UserSelectedExtraOption> SelectedExtraOptions { get; set; } =
        new List<UserSelectedExtraOption>();

    public ICollection<UserSelectedSnack> SelectedSnacks { get; set; } =
        new List<UserSelectedSnack>();

    public ICollection<UserSelectedMeal> SelectedMeals { get; set; } = new List<UserSelectedMeal>();
    public PlanOrderStatus DayOrderStatus { get; set; }
}