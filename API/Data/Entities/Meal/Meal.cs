using AsparagusN.Enums;
using AsparagusN.Interfaces;

namespace AsparagusN.Data.Entities.Meal;

public class Meal : ISoftDeletable
{
    private ICollection<MealIngredient> _ingredients = new List<MealIngredient>();
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
    public int? LoyaltyPoints { get; set; }

    public ICollection<MealIngredient> Ingredients
    {
        get => _ingredients;
        set
        {
            _ingredients = value;
           
            CalcCarb();
            CalcProtein();
            CalcFat();
            CalcFiber();
            PricePerCarb = _pricePer(IngredientType.Carb);
            PricePerProtein = _pricePer(IngredientType.Protein);
        }
    }

    public ICollection<MealAllergy> Allergies { get; set; } = new List<MealAllergy>();

    public Category Category { get; set; }
    public int CategoryId { get; set; }

    public decimal Protein{ get; private set; }
    public decimal Carbs{ get; private set; }

    public decimal Fats{ get; private set; }
    public decimal Fibers{ get; private set; }
    public decimal PricePerProtein{ get; private set; }
    public decimal PricePerCarb{ get; private set; }
    public decimal Calories() => Protein * 4 + Carbs * 4 + Fats * 9;

    public bool IsAvailable { get; set; } = true;
    public bool IsDeleted { get; set; }

    private void CalcProtein()
    {
        Protein = Ingredients.Sum(i => i.Weight * _getPriceForItem(i.Ingredient.Weight, i.Ingredient.Protein));
    }

    private void CalcCarb()
    {
        Carbs = Ingredients.Sum(i => i.Weight * _getPriceForItem(i.Ingredient.Weight, i.Ingredient.Carb));
    }

    private void CalcFat()
    {
        Fats = Ingredients.Sum(i => i.Weight * _getPriceForItem(i.Ingredient.Weight, i.Ingredient.Fat));
    }

    private void CalcFiber()
    {
        Fibers = Ingredients.Sum(i => i.Weight * _getPriceForItem(i.Ingredient.Weight, i.Ingredient.Fiber));
    }

    private decimal _getPriceForItem(decimal ingredientWeight, decimal tmp)
    {
        if (ingredientWeight == 0 || tmp == 0) return 0;
        return tmp / ingredientWeight;
    }

    private decimal _pricePer(IngredientType type)
    {
        decimal w = 0;
        decimal pri = 0;
        foreach (var item in Ingredients)
        {
            if (item.Ingredient.TypeOfIngredient != type) continue;
            w = item.Ingredient.Weight;
            pri = item.Ingredient.Price;
            break;
        }

        if (w != 0) return pri / w;
        return 0;
    }
}