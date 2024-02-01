using System.Linq.Expressions;
using AsparagusN.Entities.MealPlan;

namespace AsparagusN.Specifications;

public class DrinkItemSpecification : BaseSpecification<DrinkItem>
{
    public DrinkItemSpecification(int drinkId,int planId) : base(x=>x.DrinkId == drinkId&& x.AdminPlanId == planId)
    {
        
    }
}