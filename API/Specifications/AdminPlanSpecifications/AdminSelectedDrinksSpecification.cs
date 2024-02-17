using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications.AdminPlanSpecifications;

public class AdminSelectedDrinksSpecification : BaseSpecification<AdminSelectedDrink>
{
    public AdminSelectedDrinksSpecification(PlanTypeEnum planTypeEnum) : base(x=>x.PlanTypeEnum == planTypeEnum)
    {
        AddInclude(x=>x.Include(y=>y.Drink));
    }
    public AdminSelectedDrinksSpecification(PlanTypeEnum planTypeEnum,int id) : base(x=>x.PlanTypeEnum == planTypeEnum && x.DrinkId == id)
    {
        AddInclude(x=>x.Include(y=>y.Drink));

    }
    public AdminSelectedDrinksSpecification(int id,PlanTypeEnum planTypeEnum) : base(x=>x.PlanTypeEnum == planTypeEnum && x.Id == id)
    {
        AddInclude(x=>x.Include(y=>y.Drink));

    }
}