using System.Linq.Expressions;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;

namespace AsparagusN.Specifications;

public class UnavailableMealsInAdminDaySpecification : BaseSpecification<UnavailableMeal>
{
    public UnavailableMealsInAdminDaySpecification(int adminDay) : base(x=>x.AdminDayId == adminDay)
    {
    }
    public UnavailableMealsInAdminDaySpecification(int adminDay,int mealId)
        : base(x=>x.AdminDayId == adminDay && mealId == x.MealId)
    {
    }
    
}