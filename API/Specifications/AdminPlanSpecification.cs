using System.Linq.Expressions;
using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.Entities.MealPlan;
using AsparagusN.Enums;
using AsparagusN.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class AdminPlanSpecification : BaseSpecification<AdminPlanDay>
{
    public AdminPlanSpecification(int dayId)
        : base(x => x.Id == dayId && x.AvailableDate >= HelperFunctions.WeekStartDay() &&
                    x.AvailableDate <= HelperFunctions.WeekEndDay())
    {
        AddInclude(x => x.Include(y => y.Meals)
            .ThenInclude(y => y.Meal).ThenInclude(d => d.Allergies).ThenInclude(h => h.Allergy));
    }

    public AdminPlanSpecification(PlanTypeEnum type)
        : base(x => x.PlanType == type && x.AvailableDate >= HelperFunctions.WeekStartDay() &&
                    x.AvailableDate <= HelperFunctions.WeekEndDay())
    {
        AddInclude(x => x.Include(y => y.Meals)
            .ThenInclude(y => y.Meal).ThenInclude(d => d.Allergies).ThenInclude(h => h.Allergy));
    }

    public AdminPlanSpecification()
        : base(x => HelperFunctions.getDatesOfCurrentWeek().Contains(x.AvailableDate))
    {
        AddInclude(x => x.Include(y => y.Meals)
            .ThenInclude(y => y.Meal));
    }
}