using AsparagusN.Enums;

namespace AsparagusN.DTOs.AdminPlanDtos;

public class NewAdminPlanDto
{
    public List<int>? MealIds { get; set; }
    public List<int>? DrinkIds { get; set; }
    public List<int>? SaladIds { get; set; }
    public List<int>? SauceIds { get; set; }
    public List<int>? NutIds { get; set; }
}