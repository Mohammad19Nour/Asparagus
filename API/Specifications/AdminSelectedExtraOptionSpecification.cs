using System.Linq.Expressions;
using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Entities;
using AsparagusN.Entities.MealPlan;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class AdminSelectedExtraOptionSpecification : BaseSpecification<AdminSelectedExtraOption>
{
    public AdminSelectedExtraOptionSpecification(PlanTypeEnum planTypeEnum) : base(x => x.PlanTypeEnum == planTypeEnum)
    {
        AddInclude(x=>x.Include(t=>t.ExtraOption));
    }

    public AdminSelectedExtraOptionSpecification(PlanTypeEnum planTypeEnum, int id) : base(x =>
        x.PlanTypeEnum == planTypeEnum && x.ExtraOptionId == id)
    {
        AddInclude(x=>x.Include(t=>t.ExtraOption));

    }
}