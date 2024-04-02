using System.Linq.Expressions;
using AsparagusN.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class CategoryWithMealSpecification : BaseSpecification<Category>
{
    public CategoryWithMealSpecification(int id) : base(x=> x.Id == id)
    {
        AddInclude(x=>x.Include(y=>y.Meals)
            .ThenInclude(b=>b.Allergies).ThenInclude(t=>t.Allergy));
        
        AddInclude(x=>x.Include(y=>y.Meals)
            .ThenInclude(c=>c.Ingredients).ThenInclude(b=>b.Ingredient));
    }
}