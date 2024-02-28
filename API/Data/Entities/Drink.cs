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
    
    public decimal Protein{ get; set; }
    public decimal Carb{ get; set; }
    public decimal Fat{ get; set; }
    public decimal Fiber{ get; set; }
    public decimal GetCalories()
    {
        return Protein * 4 + Carb * 4 + Fat * 9;
    }
  //  public bool IsSelectedForPlan { get; set; } = false;
}