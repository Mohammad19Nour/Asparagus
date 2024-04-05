using AsparagusN.Data.Entities.Meal;
using AsparagusN.DTOs.PackageDtos;

namespace AsparagusN.DTOs;

public class MonthGiftDto
{
    public int Month { get; set; } // Month number (1-12)
    public string MonthName { get; set; }
    public MealInfoDto? Meal { get; set; }
}