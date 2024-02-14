using System.Linq.Expressions;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications.AdminPlanSpecifications;

public class AdminSelectedSnackSpecification : BaseSpecification<AdminSelectedSnack>
{
    public AdminSelectedSnackSpecification(PlanTypeEnum planType)
        : base(x => x.PlanTypeEnum == planType)
    {
        AddInclude(x => x.Include(y => y.Snack));
    }

    public AdminSelectedSnackSpecification(int id)
        : base(x => x.Id == id)
    {
        AddInclude(x => x.Include(y => y.Snack));
    }
}