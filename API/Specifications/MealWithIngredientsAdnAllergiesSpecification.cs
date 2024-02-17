using System.Linq.Expressions;
using AsparagusN.Data.Entities.Meal;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;
// here they came with snacks

public class MealWithIngredientsAdnAllergiesSpecification : BaseSpecification<Meal>
{
    public MealWithIngredientsAdnAllergiesSpecification(int mealId) : 
        base(x=> x.Id == mealId && !x.IsDeleted)
    {
        AddInclude(x=>x.Include(y=>y.Ingredients).ThenInclude(y=>y.Ingredient));
        AddInclude(x=>x.Include(y=>y.Allergies).ThenInclude(h=>h.Allergy));
    }
    public MealWithIngredientsAdnAllergiesSpecification() : 
        base(x=> !x.IsDeleted && x.IsMainMenu)
    {
        AddInclude(x=>x.Include(y=>y.Ingredients).ThenInclude(y=>y.Ingredient));
        AddInclude(x=>x.Include(y=>y.Allergies).ThenInclude(h=>h.Allergy));
    }
    
    public MealWithIngredientsAdnAllergiesSpecification(bool mealPlansOnly,bool includeIngredients = false) 
        : base(x=> !x.IsDeleted && x.IsMealPlan )
    {
        if (includeIngredients)
        {
            AddInclude(x => x.Include(y => y.Ingredients).ThenInclude(d => d.Ingredient));
            AddInclude(x => x.Include(y => y.Allergies).ThenInclude(h=>h.Allergy));
        }
    }
}