using System.Linq.Expressions;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications.UserSpecifications;

public class UserPlanWithMealsDrinksAndExtrasSpecification : BaseSpecification<UserPlan>
{
    public UserPlanWithMealsDrinksAndExtrasSpecification(int userId, PlanTypeEnum planType)
        : base(x => x.PlanType == planType && userId == x.AppUserId)
    {
        AddInclude(x => x.Include(y => y.Days).ThenInclude(
            d => d.SelectedDrinks));

        AddInclude(x => x.Include(y => y.Days).ThenInclude(
            d => d.SelectedMeals));
        AddInclude(x => x.Include(y => y.Days).ThenInclude(
            d => d.SelectedExtraOptions));
        AddInclude(x => x.Include(y => y.Days)
            .ThenInclude(
                d => d.SelectedMeals)
            .ThenInclude(c => c.ChangedCarb));
    }
    
}