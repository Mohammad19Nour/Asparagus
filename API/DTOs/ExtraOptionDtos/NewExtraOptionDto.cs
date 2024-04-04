using System.Text.Json.Serialization;
using AsparagusN.Enums;

namespace AsparagusN.DTOs.ExtraOptionDtos;

public class NewExtraOptionDto
{
    public string NameArabic { get; set; }
    public string NameEnglish { get; set; }
    public decimal Price { get; set; }
    public decimal Weight { get; set; }
    public IFormFile Image { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ExtraOptionType OptionType { get; set; }
    public decimal Protein{ get; set; }
    public decimal Carb{ get; set; }
    public decimal Fat{ get; set; }
    public decimal Fiber{ get; set; }
}