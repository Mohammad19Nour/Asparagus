using System.Linq.Expressions;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications.UserSpecifications;

public class UserPlanDayWithDrinksAndMealsAndExtra : BaseSpecification<UserPlanDay>
{
    public UserPlanDayWithDrinksAndMealsAndExtra(int dayId , int userId)
        : base(x => x.Id == dayId && x.UserPlan.AppUserId == userId)
    {
        AddInclude(x => x.Include(
            d => d.SelectedMeals).ThenInclude(y=>y.ChangedCarb));
        AddInclude(x => x.Include(
            d => d.SelectedDrinks));
        AddInclude(x => x.Include(
            d => d.SelectedExtraOptions));
        AddInclude(x=>x.Include(
            d=>d.UserPlan));
        AddInclude(x=>x.Include(y=>y.SelectedSnacks));
        AddInclude(x=>x.Include(y=>y.DeliveryLocation));
    }
}