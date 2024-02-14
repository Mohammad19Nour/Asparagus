namespace AsparagusN.Data.Entities.MealPlan.UserPlan;

public class UserMealCarb
{
    public int Id { get; set; }
    public string NameEN { get; set; }
    public string NameAR { get; set; }
    public string DescriptionEN { get; set; }
    public string DescriptionAR { get; set; }
    public string PictureUrl { get; set; }
    public decimal Protein;
    public decimal Carbs;
    public decimal Fats;
    public decimal Fibers;
    public decimal Calories;
}