using AsparagusN.Enums;
using AsparagusN.Interfaces;

namespace AsparagusN.Data.Entities.Meal;

public class Ingredient : ISoftDeletable
{
    public Ingredient()
    {
    }

    public int Id { get; set; }
    public string NameEN { get; set; } = "";
    public string NameAR { get; set; } = "";
    public string ExtraInfo { get; set; } = "";
    public decimal Weight { get; set; }
    public decimal Price { get; set; }
    public decimal Protein { get; set; } = 0.0m;
    public decimal Carb { get; set; } = 0.0m;
    public decimal Fat { get; set; } = 0.0m;
    public decimal Fiber { get; set; } = 0.0m;
    public IngredientType TypeOfIngredient { get; set; } = IngredientType.Carb;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public Ingredient(string nameEN, string nameAR, string extraInfo, decimal weight, decimal price,
        decimal protein, decimal carb, decimal fat, decimal fiber, IngredientType typeOfIngredient, DateTime? createdAt = null)
    {
        NameEN = nameEN;
        NameAR = nameAR;
        ExtraInfo = extraInfo;
        Weight = weight;
        Price = price;
        Protein = protein;
        Carb = carb;
        Fat = fat;
        Fiber = fiber;
        TypeOfIngredient = typeOfIngredient;
        CreatedAt = createdAt ?? DateTime.Now;
    }

    public bool IsDeleted { get; set; }
}