﻿using System.Linq.Expressions;
using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.Entities.MealPlan;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class AdminPlanDaysWithMealsSpecification : BaseSpecification<AdminPlanDay>
{
    public AdminPlanDaysWithMealsSpecification(int dayId)
        : base(x => x.Id == dayId)
    {
        AddInclude(x => x.Include(y => y.Meals).ThenInclude(d=>d.Meal));
    }

    public AdminPlanDaysWithMealsSpecification(PlanTypeEnum planType)
        : base(x => x.PlanType == planType)
    {
        AddInclude(x => x.Include(y => y.Meals).ThenInclude(d=>d.Meal));
    }
    
    public AdminPlanDaysWithMealsSpecification(DateTime dayDate, PlanTypeEnum planType)
        : base(x => x.AvailableDate == dayDate && planType == x.PlanType)
    {
        AddInclude(x => x.Include(y => y.Meals).ThenInclude(d=>d.Meal));
    }
}