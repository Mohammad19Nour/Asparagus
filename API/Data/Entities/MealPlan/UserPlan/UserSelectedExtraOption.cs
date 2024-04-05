using AsparagusN.Enums;

namespace AsparagusN.Data.Entities.MealPlan.UserPlan;

public class UserSelectedExtraOption
{
    public int Id { get; set; }
    public int UserPlanDayId { get; set; }
    public UserPlanDay UserPlanDay { get; set; }
    
    public string NameArabic { get; set; }
    public string NameEnglish { get; set; }
    public decimal Weight { get; set; }
    public string PictureUrl { get; set; }
    public ExtraOptionType OptionType { get; set; }
    public decimal Price { get; set; }
    public decimal Protein{ get; set; }
    public decimal Carb{ get; set; }
    public decimal Fat{ get; set; }
    public decimal Fiber{ get; set; }
    public decimal GetCalories()
    {
        return Protein * 4 + Carb * 4 + Fat * 9;
    }
}