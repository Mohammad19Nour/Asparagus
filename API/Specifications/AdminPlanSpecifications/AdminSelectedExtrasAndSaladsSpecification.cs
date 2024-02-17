using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications.AdminPlanSpecifications;

public class AdminSelectedExtrasAndSaladsSpecification:BaseSpecification<AdminSelectedExtraOption>
{
    public AdminSelectedExtrasAndSaladsSpecification(ExtraOptionType optionType, PlanTypeEnum planType) 
        : base(x=>x.ExtraOption.OptionType == optionType && planType == x.PlanTypeEnum)
    {
        AddInclude(x=>x.Include(y=>y.ExtraOption));
    }
    public AdminSelectedExtrasAndSaladsSpecification(PlanTypeEnum planType) 
        : base(x=>planType == x.PlanTypeEnum)
    {
        AddInclude(x=>x.Include(y=>y.ExtraOption));
    }
}