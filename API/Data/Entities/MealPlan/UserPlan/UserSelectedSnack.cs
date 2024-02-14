namespace AsparagusN.Data.Entities.MealPlan.UserPlan;

public class UserSelectedSnack
{
    public int Id { get; set; }
    public string NameEN { get; set; }
    public string NameAR { get; set; }
    public string DescriptionEN { get; set; }
    public string DescriptionAR { get; set; }
    public string PictureUrl { get; set; }
    public decimal Protein{ get; set; }
    public decimal Carbs{ get; set; }
    public decimal Fats{ get; set; }
    public decimal Fibers{ get; set; }
    public decimal Calories{ get; set; }
    public int Quantity { get; set; }
}