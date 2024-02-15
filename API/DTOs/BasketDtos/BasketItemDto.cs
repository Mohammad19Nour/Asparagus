using System.ComponentModel.DataAnnotations;

namespace AsparagusN.DTOs.BasketDtos;

public class BasketItemDto
{
    public int MealId { get; set; }
    public string NameEN { get; set; }
    public string NameAR { get; set; }
    [Required]
    [Range(0.01,double.MaxValue,ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
    [Required]
    [Range(1,double.MaxValue,ErrorMessage = "Quantity should be at least 1")]
    public int Quantity { get; set; }
    public int AddedCarb { get; set; }
    public int AddedProtein { get; set; }
    public string PictureUrl { get; set; }
}