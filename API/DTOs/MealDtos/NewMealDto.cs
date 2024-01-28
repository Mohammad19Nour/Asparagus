using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.DTOs.MealDtos;

public class NewMealDto
{
    public string NameEN { get; set; }
    public string NameAR { get; set; }
    public string DescriptionEN { get; set; }
    public string DescriptionAR { get; set; }
    public decimal Price { get; set; }
    public int Points { get; set; }
    public List<int>? Allergies { get; set; } = new List<int>();
    public List<MealIngredientDto> Ingredients { get; set; }
    public int CategoryId { get; set; }
    [Required(ErrorMessage = "Image is required")]
    public IFormFile ImageFile { get; set; }
}   