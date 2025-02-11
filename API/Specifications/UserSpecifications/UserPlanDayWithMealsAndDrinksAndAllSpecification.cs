﻿using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications.UserSpecifications;

public class UserPlanDayWithMealsAndDrinksAndAllSpecification : BaseSpecification<UserPlanDay>
{
    public UserPlanDayWithMealsAndDrinksAndAllSpecification(int userId, int dayId)
        : base(x => x.Id == dayId && x.UserPlan.AppUserId == userId)
    {
        AddInclude(x => x.Include(y => y.UserPlan).ThenInclude(p=>p.Allergies));
        AddInclude(x => x.Include(y => y.SelectedDrinks));
        AddInclude(x => x.Include(y => y.SelectedExtraOptions));
        AddInclude(x => x.Include(y => y.SelectedSnacks));
        AddInclude(x => x.Include(y => y.SelectedMeals).ThenInclude(y=>y.ChangedCarb));
    }
    public UserPlanDayWithMealsAndDrinksAndAllSpecification(DateTime dayDate) // for admin only (packaging)
        : base(x => x.Day.Date == dayDate && x.DayOrderStatus != PlanOrderStatus.Delivered)
    {
        AddInclude(x => x.Include(y => y.UserPlan).ThenInclude(c=>c.Allergies));
        AddInclude(x => x.Include(y => y.SelectedDrinks));
        AddInclude(x => x.Include(y => y.SelectedExtraOptions));
        AddInclude(x => x.Include(y => y.SelectedSnacks));
        AddInclude(x => x.Include(y => y.SelectedMeals).ThenInclude(y=>y.ChangedCarb));
        
    }
}