namespace AsparagusN.DTOs.AllergyDtos;

public class UpdateAllergyDto
{
    public string? ArabicName { get; set; }
    public string? EnglishName { get; set; }
    public IFormFile? Image { get; set; }
}