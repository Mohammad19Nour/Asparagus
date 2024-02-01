using System.Linq.Expressions;
using AsparagusN.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class MealWithIngredientsAdnAllergiesSpecification : BaseSpecification<Meal>
{
    public MealWithIngredientsAdnAllergiesSpecification(int mealId, int branchId) : 
        base(x=> x.Id == mealId && !x.IsDeleted && x.BranchId == branchId && x.IsMainMenu)
    {
        AddInclude(x=>x.Include(y=>y.Ingredients));
        AddInclude(x=>x.Include(y=>y.Allergies));
    }
    public MealWithIngredientsAdnAllergiesSpecification(int branchId) 
        : base(x=> x.BranchId == branchId && !x.IsDeleted && x.IsMainMenu )
    {
        AddInclude(x=>x.Include(y=>y.Ingredients).ThenInclude(d=>d.Ingredient));
        AddInclude(x=>x.Include(y=>y.Allergies));
    }
    
    public MealWithIngredientsAdnAllergiesSpecification(bool mealPlansOnly,bool includeIngredients = false) 
        : base(x=> !x.IsDeleted && x.IsMealPlan )
    {
        if (includeIngredients)
        {
            AddInclude(x => x.Include(y => y.Ingredients).ThenInclude(d => d.Ingredient));
            AddInclude(x => x.Include(y => y.Allergies));
        }
    }
}