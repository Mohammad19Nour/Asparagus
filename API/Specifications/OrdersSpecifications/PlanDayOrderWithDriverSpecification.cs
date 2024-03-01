using System.Linq.Expressions;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.Data.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications.OrdersSpecifications;

public class PlanDayOrderWithDriverSpecification : BaseSpecification<UserPlanDay>
{
    public PlanDayOrderWithDriverSpecification(int orderId)
        : base(x=>orderId == x.Id)
    {
        AddInclude(x=>x.Include(y=>y.Driver));
    }
}