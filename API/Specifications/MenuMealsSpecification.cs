using System.Linq.Expressions;
using AsparagusN.Data.Entities.Meal;

namespace AsparagusN.Specifications;

public class MenuMealsSpecification : BaseSpecification<Meal>
{
    public MenuMealsSpecification() : base(x=>!x.IsDeleted && x.IsMainMenu)
    {
    }
    
    public MenuMealsSpecification(int id) : base(x=>!x.IsDeleted && x.IsMainMenu && x.Id == id)
    {
    }
}