using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Data.Entities.OrderAggregate;
[Owned]
public class MealItemOrdered
{
    public int OrderItemId { get; set; }
    public int MealId { get; set; }
    public string NameEN { get; set; }
    public string NameAR { get; set; }
    public string DescriptionEN { get; set; }
    public string DescriptionAR { get; set; }
    public string PictureUrl { get; set; }
    public double PricePerProtein { get; set; } = 0; 
    public double PricePerCarb { get; set; } = 0; 
    public double Calories { get; set; } = 0; 
    public double Fibers { get; set; } = 0; 
    public double Fats { get; set; } = 0; 
    public double Carbs { get; set; } = 0; 
    public double Protein { get; set; } = 0; 
    public int AddedCarb { get; set; }
    public int AddedProtein { get; set; }

}