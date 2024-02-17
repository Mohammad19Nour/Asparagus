using AsparagusN.Entities.Identity;
using AsparagusN.Entities.MealPlan;
using AsparagusN.Enums;

namespace AsparagusN.Data.Entities.MealPlan.UserPlan;

public class UserPlan
{
    public int Id { get; set; }
    public AppUser User { get; set; }
    public int AppUserId { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime StartDate { get; set; } = DateTime.Now.AddDays(2);
    public int Duration { get; set; }
    public PlanTypeEnum PlanType { get; set; }
    public int NumberOfMealPerDay { get; set; }
    public int NumberOfSnacks { get; set; }
    public int NumberOfRemainingSnacks { get; set; }
    public List<UserPlanDay> Days { get; set; } = new List<UserPlanDay>();
    public List<UserPlanAllergy> Allergies { get; set; } = new List<UserPlanAllergy>();
    public string? Notes { get; set; }
    public string DeliveryCity { get; set; }

    public DateTime EndDate()
    {
        return StartDate.AddDays(Duration);
    }

    public UserPlan()
    {
        NumberOfRemainingSnacks = NumberOfSnacks;
    }
}