using AsparagusN.Enums;

namespace AsparagusN.DTOs.UserPlanDtos;

public class UserSelectedExtraOptionDto
{
    public int Id { get; set; }

    public string NameArabic { get; set; }
    public string NameEnglish { get; set; }
    public decimal Weight { get; set; }
    public string PictureUrl { get; set; }
    public string OptionType { get; set; }
    public decimal Price { get; set; }
}