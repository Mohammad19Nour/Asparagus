namespace AsparagusN.DTOs.MealDtos;

public class UpdateMealDto
{
    public string? NameEN { get; set; }
    public string? NameAR { get; set; }
    public string? DescriptionEN { get; set; }
    public string? DescriptionAR { get; set; }
    public decimal? Price { get; set; }
    public int? Points { get; set; }
    public List<int>? Allergies { get; set; }
    public List<MealIngredientDto>? Ingredients { get; set; }
    
    public IFormFile? ImageFile { get; set; }
    public int? CategoryId { get; set; }
}