using System.Linq.Expressions;
using AsparagusN.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class CategoryWithMealSpecification : BaseSpecification<Category>
{
    public CategoryWithMealSpecification(int id) : base(x=> x.Id == id)
    {
        AddInclude(x=>x.Include(y=>y.Meals)
            .ThenInclude(x=>x.Allergies));
        
        AddInclude(x=>x.Include(y=>y.Meals)
            .ThenInclude(x=>x.Ingredients).ThenInclude(x=>x.Ingredient));
    }
}