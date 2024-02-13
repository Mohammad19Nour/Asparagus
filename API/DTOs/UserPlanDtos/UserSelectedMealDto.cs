namespace AsparagusN.DTOs.UserPlanDtos;

public class UserSelectedMealDto
{
    public int Id { get; set; }
    
    public string NameEN { get; set; }
    public string NameAR { get; set; }
    public string DescriptionEN { get; set; }
    public string DescriptionAR { get; set; }
    public string PictureUrl { get; set; }
    public decimal PricePerProtein { get; set; }
    public decimal PricePerCarb { get; set; }
    public decimal Calories{ get; set; }
    public decimal Fibers{ get; set; }
    public decimal Fats{ get; set; }
    public decimal Carbs{ get; set; }
    public decimal Protein{ get; set; }
    public int AddedCarb { get; set; }
    public int AddedProtein { get; set; }
}