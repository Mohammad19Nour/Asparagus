using AsparagusN.DTOs.AllergyDtos;

namespace AsparagusN.DTOs.MealDtos;

public class MealDto
{  
    public int Id { get; set; }
    public string NameEN { get; set; }
    public string NameAR { get; set; }
    public string DescriptionEN { get; set; }
    public string DescriptionAR { get; set; }
    public decimal Price { get; set; }
    public int Points { get; set; }
    public string PictureUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal Protein => Ingredients.Sum(i=> i.Weight * _getPriceForItem(i.Ingredient.Weight,i.Ingredient.Protein) );

    public decimal Carb => Ingredients.Sum(i=> i.Weight * _getPriceForItem(i.Ingredient.Weight,i.Ingredient.Carb) );

    public decimal Fat => Ingredients.Sum(i=> i.Weight * _getPriceForItem(i.Ingredient.Weight,i.Ingredient.Fat) );

    public decimal Fibers=> Ingredients.Sum(i=> i.Weight * _getPriceForItem(i.Ingredient.Weight,i.Ingredient.Fiber) );
    public decimal Calories => Protein * 4 + Carb * 4 + Fat * 9;    
    public int CategoryId { get; set; }
    public IEnumerable<MealIngredientDetailsDto>Ingredients { get; set; }
    public IEnumerable<AllergyDto> Allergies { get; set; }
    public int BranchId { get; set; }
    private decimal _getPriceForItem(decimal ingredientWeight,decimal tmp)
    {
        if (ingredientWeight == 0 || tmp == 0) return 0;
        return tmp / ingredientWeight;
        
    }
}