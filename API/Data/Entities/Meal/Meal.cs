using System.ComponentModel.DataAnnotations.Schema;

namespace AsparagusN.Entities;

public class Meal
{
    public int Id { get; set; }
    public string NameEN { get; set; }
    public string NameAR { get; set; }
    public string DescriptionEN { get; set; }
    public string DescriptionAR { get; set; }
    public decimal Price { get; set; }
    public int Points { get; set; }
    public string PictureUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsMealPlan { get; set; }
    public bool IsMainMenu { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<MealIngredient> Ingredients { get; set; } = new List<MealIngredient>();
    public ICollection<MealAllergy> Allergies { get; set; } = new List<MealAllergy>();

    public Category Category { get; set; }
    public int CategoryId { get; set; }

    public decimal Protein() => Ingredients.Sum(i=> i.Weight * _getPriceForItem(i.Ingredient.Weight,i.Ingredient.Protein) );

    public decimal Carbs()=> Ingredients.Sum(i=> i.Weight * _getPriceForItem(i.Ingredient.Weight,i.Ingredient.Carb) );

    
    public decimal Fats() => Ingredients.Sum(i=> i.Weight * _getPriceForItem(i.Ingredient.Weight,i.Ingredient.Fat) );

    public decimal Fibers ()=> Ingredients.Sum(i=> i.Weight * _getPriceForItem(i.Ingredient.Weight,i.Ingredient.Fiber) );

    public decimal Calories() =>Protein() * 4 + Carbs() * 4 + Fats() * 9;
    private decimal _getPriceForItem(decimal ingredientWeight,decimal tmp)
    {
        if (ingredientWeight == 0 || tmp == 0) return 0;
        return tmp / ingredientWeight;
    }
}