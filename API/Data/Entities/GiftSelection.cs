namespace AsparagusN.Data.Entities;

public class GiftSelection
{
    public int Id { get; set; }
    public int Month { get; set; } // Month number (1-12)

    public int? MealId { get; set; }
    public Meal.Meal? Meal { get; set; }
}