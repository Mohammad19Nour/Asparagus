using AsparagusN.Enums;
using AsparagusN.Interfaces;

namespace AsparagusN.Entities;

public class ExtraOption : ISoftDeletable
{
    public int Id { get; set; }
    public string NameArabic { get; set; }
    public string NameEnglish { get; set; }
    public decimal Price { get; set; }
    public decimal Weight { get; set; }
    public string PictureUrl { get; set; }
    public bool IsDeleted { get; set; }
    public ExtraOptionType OptionType { get; set; }
}
