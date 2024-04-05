using System.Linq.Expressions;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications.OrdersSpecifications;

public class PlanDayOrdersWithItemsAndDriverSpecification : BaseSpecification<UserPlanDay>
{
    public PlanDayOrdersWithItemsAndDriverSpecification(PlanOrderStatus status, DateTime day)
        : base(x => x.DayOrderStatus == status && x.Day.Date == day.Date)
    {
        AddInclude(c=>c.Include(d=>d.DeliveryLocation));
        AddInclude(c=>c.Include(d=>d.Driver));
        AddInclude(x => x.Include(y => y.UserPlan).ThenInclude(p=>p.Allergies));
        AddInclude(x => x.Include(y => y.UserPlan).ThenInclude(p=>p.User));
        AddInclude(x => x.Include(y => y.SelectedDrinks));
        AddInclude(x => x.Include(y => y.SelectedExtraOptions));
        AddInclude(x => x.Include(y => y.SelectedSnacks));
        AddInclude(x => x.Include(y => y.SelectedMeals).ThenInclude(y=>y.ChangedCarb));

    }
}