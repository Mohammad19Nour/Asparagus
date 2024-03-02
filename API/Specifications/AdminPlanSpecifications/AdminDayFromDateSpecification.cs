using System.Linq.Expressions;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.Enums;

namespace AsparagusN.Specifications.AdminPlanSpecifications;

public class AdminDayFromDateSpecification : BaseSpecification<AdminPlanDay>
{
    public AdminDayFromDateSpecification(DateTime dayDate,PlanTypeEnum planTypeEnum) 
        : base(x=>x.AvailableDate.Date == dayDate.Date && planTypeEnum == x.PlanType)
    {
    }
}