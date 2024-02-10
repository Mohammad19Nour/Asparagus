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
    public decimal PricePerProtein;
    public decimal PricePerCarb;
    public decimal Calories;
    public decimal Fibers;
    public decimal Fats;
    public decimal Carbs;
    public decimal Protein;
    public int AddedCarb { get; set; }
    public int AddedProtein { get; set; }

}