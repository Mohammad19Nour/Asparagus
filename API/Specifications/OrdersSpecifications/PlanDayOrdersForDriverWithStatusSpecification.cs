using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications.OrdersSpecifications;

public class PlanDayOrdersForDriverWithStatusSpecification : BaseSpecification<UserPlanDay>
{
    public PlanDayOrdersForDriverWithStatusSpecification(int driverId,PlanOrderStatus status) 
        : base(x=>x.DriverId == driverId && status == x.DayOrderStatus)
    {
        AddInclude(x=>x.Include(y=>y.DeliveryLocation));
    }
     public PlanDayOrdersForDriverWithStatusSpecification(int driverId,PlanOrderStatus status,bool forNotification) 
        : base(x=>x.DriverId == driverId && status == x.DayOrderStatus)
    {
        AddInclude(x=>x.Include(y=>y.UserPlan));
    }
}