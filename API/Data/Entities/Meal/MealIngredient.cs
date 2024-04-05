namespace AsparagusN.Data.Entities.Meal;

public class MealIngredient
{
    public MealIngredient()
    {
    }

    public int MealId { get; set; }
    public int IngredientId { get; set; }
    public Meal Meal { get; set; }
    public Ingredient Ingredient { get; set; }
    public decimal Weight { get; set; }
}