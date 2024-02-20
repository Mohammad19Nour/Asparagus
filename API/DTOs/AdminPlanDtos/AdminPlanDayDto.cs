using AsparagusN.DTOs.MealDtos;
using AsparagusN.Enums;

namespace AsparagusN.DTOs.AdminPlanDtos;

public class AdminPlanDayDto
{
    public int Id { get; set; }
    public DateTime AvailableDate { get; set; }
    public string PlanType { get; set; }
    public List<MealWithIngredientsDto> Meals { get; set; }
   
}