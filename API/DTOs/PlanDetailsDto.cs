using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.DTOs.AdditionDtos;
using AsparagusN.DTOs.AdminPlanDtos;
using AsparagusN.DTOs.DrinksDtos;
using AsparagusN.DTOs.MealDtos;
using AsparagusN.Enums;

namespace AsparagusN.DTOs;

public class PlanDetailsDto
{
    public string? PlanType { get; set; }
    public int Points { get; set; }
    public List<AdminPlanDayDto> Days { get; set; }
    public List<DrinkDto> Drinks { get; set; }
    public List<ExtraOptionDto> ExtraOptionDtos { get; set; }
    public List<SnackDto> Snacks { get; set; }
}