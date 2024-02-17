using System.ComponentModel.DataAnnotations;
using AsparagusN.Entities;

namespace AsparagusN.Data.Entities;

public class BasketItem
{
    public int CustomerBasketId { get; set; }
    public CustomerBasket CustomerBasket { get; set; }
    public Meal Meal { get; set; }
    public int MealId { get; set; }

    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "Quantity should be at least 1")]
    public int Quantity { get; set; }

    public int AddedCarb { get; set; }
    public int AddedProtein { get; set; }

    public string Note { get; set; }
    public bool RemoveSauce { get; set; }
}