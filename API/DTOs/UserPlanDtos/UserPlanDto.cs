using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs.AllergyDtos;
using AsparagusN.Enums;

namespace AsparagusN.DTOs.UserPlanDtos;

public class UserPlanDto
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime StartDate { get; set; }
    public int Duration { get; set; }
    public string PlanType { get; set; }
    public int NumberOfMealPerDay { get; set; }
    public int NumberOfSnacks { get; set; }
    public int NumberOfRemainingSnacks { get; set; }
    public List<UserPlanDayDto> Days { get; set; }
    public List<AllergyDto> Allergies { get; set; }

    public DateTime EndDate { get; set; }
}