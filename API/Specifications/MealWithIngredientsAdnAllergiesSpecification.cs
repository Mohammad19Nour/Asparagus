using System.Linq.Expressions;
using AsparagusN.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class MealWithIngredientsAdnAllergiesSpecification : BaseSpecification<Meal>
{
    public MealWithIngredientsAdnAllergiesSpecification(int id) : base(x=> x.Id == id && !x.IsDeleted)
    {
        AddInclude(x=>x.Include(y=>y.Ingredients));
        AddInclude(x=>x.Include(y=>y.Allergies));
    }
    public MealWithIngredientsAdnAllergiesSpecification(bool isMenu = false ) 
        : base(x=>!x.IsDeleted && (isMenu ? x.IsMainMenu:x.IsMealPlan))
    {
        AddInclude(x=>x.Include(y=>y.Ingredients).ThenInclude(d=>d.Ingredient));
        AddInclude(x=>x.Include(y=>y.Allergies));
        
    }
}