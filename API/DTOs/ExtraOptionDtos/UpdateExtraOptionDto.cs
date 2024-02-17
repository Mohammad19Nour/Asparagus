namespace AsparagusN.DTOs.ExtraOptionDtos;

public class UpdateExtraOptionDto
{
    public string? NameArabic { get; set; }
    public string? NameEnglish { get; set; }
    public decimal? Price { get; set; }
    public decimal? Weight { get; set; }
    public IFormFile? Image { get; set; }
}