using System.Linq.Expressions;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.Enums;
using AsparagusN.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications.UserSpecifications;

public class UserPlanWithMealsDrinksAndExtrasSpecification : BaseSpecification<UserPlan>
{
    public UserPlanWithMealsDrinksAndExtrasSpecification(int userId, PlanTypeEnum planType)
        : base(x => x.PlanType == planType && userId == x.AppUserId 
                                           && x.StartDate.Date <= HelperFunctions.WeekEndDay() && x.StartDate.Date.AddDays(x.Duration) > HelperFunctions.WeekStartDay())
    {
        Console.WriteLine(HelperFunctions.WeekEndDay());
        Console.WriteLine("\n*\n");
        AddInclude(x => x.Include(y => y.Allergies));
        AddInclude(x => x.Include(y => y.Days).ThenInclude(
            d => d.SelectedDrinks));
        AddInclude(x => x.Include(y => y.Days).ThenInclude(
            d => d.DeliveryLocation));
        AddInclude(x => x.Include(y => y.Days).ThenInclude(
            d => d.SelectedSnacks));
        AddInclude(x => x.Include(y => y.Days).ThenInclude(
            d => d.SelectedMeals));
        AddInclude(x => x.Include(y => y.Days).ThenInclude(
            d => d.SelectedExtraOptions));
        AddInclude(x => x.Include(y => y.Days)
            .ThenInclude(
                d => d.SelectedMeals).ThenInclude(y=>y.ChangedCarb));
    }
}