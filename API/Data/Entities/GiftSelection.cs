namespace AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Meal;
public class GiftSelection
{
    public int Id { get; set; }
    public int Month { get; set; } // Month number (1-12)
    public string MonthName { get; set; }
    public int? MealId { get; set; }
    public Meal.Meal? Meal { get; set; }
}