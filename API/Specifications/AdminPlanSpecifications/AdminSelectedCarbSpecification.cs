using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications.AdminPlanSpecifications;

public class AdminSelectedCarbSpecification : BaseSpecification<AdminSelectedCarb>
{
    public AdminSelectedCarbSpecification(PlanTypeEnum planType) : base(x => x.PlanTypeEnum == planType)
    {
        AddInclude(x => x.Include(y => y.Carb));
    }
    public AdminSelectedCarbSpecification(int id) : base(x => x.Id == id)
    {
        AddInclude(x => x.Include(y => y.Carb));
    }
}