using System.ComponentModel.DataAnnotations;

namespace AsparagusN.Entities;

public class BasketItem
{
    [Required]
    public int Id { get; set; }
    [Required]

    public string ProductName { get; set; }
    [Required]
    [Range(0.01,double.MaxValue,ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
    [Required]
    [Range(1,double.MaxValue,ErrorMessage = "Quantity should be at least 1")]
    public int Quantity { get; set; }
    [Required]
    public string PictureUrl { get; set; }
}