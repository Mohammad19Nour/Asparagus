namespace AsparagusN.Data.Entities.MealPlan.UserPlan;

public class UserMealCarb
{
    public int Id { get; set; }
    public string NameEN { get; set; } = "This meal doesn't have carb";
    public string NameAR { get; set; } = "لا تحتوي هذه الوجبة على كارب";
    public decimal Protein { get; set; } = 0;
    public decimal Carb { get; set; } = 0;
    public decimal Fat { get; set; } = 0;
    public decimal Fiber { get; set; } = 0;

}