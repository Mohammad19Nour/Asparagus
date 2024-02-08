using System.Linq.Expressions;
using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Entities.MealPlan;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class AdminSelectedDrinksSpecification : BaseSpecification<AdminSelectedDrink>
{
    public AdminSelectedDrinksSpecification(PlanType planType) : base(x=>x.PlanType == planType)
    {
        AddInclude(x=>x.Include(y=>y.Drink));
    }
    public AdminSelectedDrinksSpecification(PlanType planType,int id) : base(x=>x.PlanType == planType && x.DrinkId == id)
    {
        AddInclude(x=>x.Include(y=>y.Drink));

    }
}