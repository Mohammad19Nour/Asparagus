using AsparagusN.Entities.Identity;
using AsparagusN.Enums;

namespace AsparagusN.Entities.MealPlan;

public class OrderedMealPlan
{
    public int Id { get; set; }
    public AppUser User { get; set; }
    
    public PlanType PlanType { get; set; }
    
    public DateTime CreatedDate { get; set; } = new DateTime();
    public DateTime StartDate { get; set; }
    public int DurationInDays { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    private List<OrderedMealPlanItem> Items;

    public DateTime EndDate()
    {
        return StartDate.AddDays(DurationInDays);
    }
}