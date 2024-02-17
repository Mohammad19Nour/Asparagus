using System.Linq.Expressions;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class UserPlanDayWithMealsAndDrinksAndAllSpecification : BaseSpecification<UserPlanDay>
{
    public UserPlanDayWithMealsAndDrinksAndAllSpecification(int userId, int dayId)
        : base(x => x.Id == dayId && x.UserPlan.AppUserId == userId)
    {
        AddInclude(x => x.Include(y => y.UserPlan));
        AddInclude(x => x.Include(y => y.SelectedDrinks));
        AddInclude(x => x.Include(y => y.SelectedExtraOptions));
        AddInclude(x => x.Include(y => y.SelectedSnacks));
        AddInclude(x => x.Include(y => y.SelectedMeals).ThenInclude(y=>y.ChangedCarb));
    }
}