namespace AsparagusN.Data.Entities.Meal;

public class MealAllergy
{
    public int MealId { get; set; }
    public int AllergyId { get; set; }
    public Meal Meal{ get; set; }
    public Allergy Allergy{ get; set; }
}