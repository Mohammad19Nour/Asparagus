using System.Linq.Expressions;
using AsparagusN.Entities;

namespace AsparagusN.Specifications;

public class SnackMealsSpecification:BaseSpecification<Meal>
{
    public SnackMealsSpecification() : base(x=>!x.IsDeleted && x.IsMealPlan)
    {
    }
}