using System.Text.Json.Serialization;
using AsparagusN.Enums;

namespace AsparagusN.DTOs.DrinksDtos;

public class UpdateDrinkDto
{
    public string? NameArabic { get; set; }
    public string? NameEnglish { get; set; }
    public decimal? Price { get; set; }
    public IFormFile? Image { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CapacityLevel? Volume { get; set; }
}