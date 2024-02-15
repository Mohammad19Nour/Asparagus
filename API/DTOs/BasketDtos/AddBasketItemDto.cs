using System.ComponentModel.DataAnnotations;

namespace AsparagusN.DTOs;

public class AddBasketItemDto
{
    [Required] public int MealId { get; set; }
    
    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "Quantity should be at least 1")]
    public int Quantity { get; set; }

    public int AddedCarb { get; set; }
    public int AddedProtein { get; set; }
    public string Note { get; set; }
    public bool RemoveSauce { get; set; }
}