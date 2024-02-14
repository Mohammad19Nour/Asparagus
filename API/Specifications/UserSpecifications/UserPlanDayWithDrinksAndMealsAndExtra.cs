﻿using System.Linq.Expressions;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.Entities.MealPlan;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications.UserSpecifications;

public class UserPlanDayWithDrinksAndMealsAndExtra : BaseSpecification<UserPlanDay>
{
    public UserPlanDayWithDrinksAndMealsAndExtra(int dayId , int userId)
        : base(x => x.Id == dayId && x.UserPlan.AppUserId == userId)
    {
        AddInclude(x => x.Include(
            d => d.SelectedMeals)
            .ThenInclude(c=>c.ChangedCarb));
        AddInclude(x => x.Include(
            d => d.SelectedDrinks));
        AddInclude(x => x.Include(
            d => d.SelectedExtraOptions));
        AddInclude(x=>x.Include(
            d=>d.UserPlan));
    }
}