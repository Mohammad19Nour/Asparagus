using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications.AdminPlanSpecifications;

public class AdminSelectedSnacksSpecification : BaseSpecification<AdminSelectedSnack>
{
    public AdminSelectedSnacksSpecification(PlanTypeEnum planTypeEnum) 
        : base(x => x.PlanTypeEnum == planTypeEnum)
    {
        AddInclude(x => x.Include(y => y.Snack));
    }

    public AdminSelectedSnacksSpecification(PlanTypeEnum planTypeEnum, int id) : base(x =>
        x.Id == id && x.PlanTypeEnum == planTypeEnum)
    {
        AddInclude(x => x.Include(y => y.Snack));
    }
}