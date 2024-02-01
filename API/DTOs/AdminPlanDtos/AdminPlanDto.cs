using AsparagusN.Data.Additions;
using AsparagusN.DTOs.AdditionDtos;
using AsparagusN.DTOs.MealDtos;
using AsparagusN.Entities.MealPlan;
using AsparagusN.Enums;

namespace AsparagusN.DTOs.AdminPlanDtos;

public class AdminPlanDto
{
    public int Id { get; set; }
    public DateTime AvailableDate { get; set; }
    public MealPlanType PlanType { get; set; }
    public List<AdminSelectedMealDto> Meals { get; set; }
   
}