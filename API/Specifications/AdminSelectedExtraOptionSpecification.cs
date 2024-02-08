using System.Linq.Expressions;
using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Entities;
using AsparagusN.Entities.MealPlan;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class AdminSelectedExtraOptionSpecification : BaseSpecification<AdminSelectedExtraOption>
{
    public AdminSelectedExtraOptionSpecification(PlanType planType) : base(x => x.PlanType == planType)
    {
        AddInclude(x=>x.Include(t=>t.ExtraOption));
    }

    public AdminSelectedExtraOptionSpecification(PlanType planType, int id) : base(x =>
        x.PlanType == planType && x.ExtraOptionId == id)
    {
        AddInclude(x=>x.Include(t=>t.ExtraOption));

    }
}