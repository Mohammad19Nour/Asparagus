using System.ComponentModel.DataAnnotations;
using AsparagusN.Enums;
using AsparagusN.Interfaces;

namespace AsparagusN.Data.Entities;

public class ExtraOption : ISoftDeletable
{
    public int Id { get; set; }
    public string NameArabic { get; set; }
    public string NameEnglish { get; set; }
    public decimal Price { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "The weight must be at least 1.")]
    public decimal Weight { get; set; }
    public string PictureUrl { get; set; }
    public bool IsDeleted { get; set; }
    public ExtraOptionType OptionType { get; set; }
}
