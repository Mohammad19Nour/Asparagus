using System.Text.Json.Serialization;
using AsparagusN.Enums;

namespace AsparagusN.DTOs.AdditionDtos;

public class NewExtraOptionDto
{
    public string NameArabic { get; set; }
    public string NameEnglish { get; set; }
    public decimal Price { get; set; }
    public decimal Weight { get; set; }
    public IFormFile Image { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ExtraOptionType OptionType { get; set; }
}