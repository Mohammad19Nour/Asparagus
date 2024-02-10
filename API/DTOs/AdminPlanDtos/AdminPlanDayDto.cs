using AsparagusN.DTOs.AdditionDtos;
using AsparagusN.DTOs.MealDtos;
using AsparagusN.Entities.MealPlan;
using AsparagusN.Enums;

namespace AsparagusN.DTOs.AdminPlanDtos;

public class AdminPlanDayDto
{
    public int Id { get; set; }
    public DateTime AvailableDate { get; set; }
    public string PlanType { get; set; }
    public List<MealWithoutIngredientsDto> Meals { get; set; }
   
}