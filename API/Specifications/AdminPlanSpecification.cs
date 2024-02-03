using System.Linq.Expressions;
using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Entities.MealPlan;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class AdminPlanSpecification : BaseSpecification<AdminPlan>
{
    public AdminPlanSpecification(int dayId) : base(x => x.Id == dayId)
    {
        AddInclude(x => x.Include(y => y.Meals)
            .ThenInclude(y=>y.Meal).ThenInclude(y=>y.Ingredients).ThenInclude(x=>x.Ingredient)
        );
        
        AddInclude(x => x.Include(y => y.Meals)
            .ThenInclude(y=>y.Meal).ThenInclude(y=>y.Allergies)
        );
        AddInclude(x => x.Include(y => y.Meals)
            .ThenInclude(y=>y.Meal).ThenInclude(y=>y.Category)
        );
    }
}