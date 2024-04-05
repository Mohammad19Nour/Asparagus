using System.Linq.Expressions;
using AsparagusN.Data.Entities.Meal;

namespace AsparagusN.Specifications;

public class IngredientNotDeletedSpecification : BaseSpecification<Ingredient>
{
    public IngredientNotDeletedSpecification(int id) : base(x => x.Id == id && !x.IsDeleted)
    {
    }

    public IngredientNotDeletedSpecification() : base(x=>!x.IsDeleted)
    {
    }
}