using AsparagusN.Enums;

namespace AsparagusN.Data.Entities.MealPlan.UserPlan;

public class UserSelectedDrink
{
    public int Id { get; set; }
    public int UserPlanDayId { get; set; }
    public UserPlanDay UserPlanDay { get; set; }
    public string NameArabic { get; set; }
    public string NameEnglish { get; set; }
    public CapacityLevel Volume { get; set; }
    public string PictureUrl { get; set; }
    public decimal Price { get; set; }
}