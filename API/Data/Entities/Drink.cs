using AsparagusN.Enums;
using AsparagusN.Interfaces;

namespace AsparagusN.Data.Entities;

public class Drink : ISoftDeletable
{
    public int Id { get; set; }
    public string NameArabic { get; set; }
    public string NameEnglish { get; set; }
    public decimal Price { get; set; }
    public CapacityLevel Volume { get; set; }
    public string PictureUrl { get; set; }
    public bool IsDeleted { get; set; }
  //  public bool IsSelectedForPlan { get; set; } = false;
}