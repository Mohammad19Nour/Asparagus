using System.Linq.Expressions;
using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Entities.MealPlan;
using AsparagusN.Enums;
using AsparagusN.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class AdminPlanSpecification : BaseSpecification<AdminPlan>
{
    public AdminPlanSpecification(int dayId) : base(x => x.Id == dayId)
    {
        AddInclude(x => x.Include(y => y.Meals)
            .ThenInclude(y => y.Meal));
    }

    public AdminPlanSpecification()
        : base(x => HelperFunctions.getDatesOfCurrentWeek().Contains(x.AvailableDate))
    {
        AddInclude(x => x.Include(y => y.Meals)
            .ThenInclude(y => y.Meal));
    }
}