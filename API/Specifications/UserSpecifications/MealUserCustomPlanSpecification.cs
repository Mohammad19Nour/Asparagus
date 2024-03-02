using System.Linq.Expressions;
using AsparagusN.Data.Entities.Meal;

namespace AsparagusN.Specifications.UserSpecifications;

public class MealUserCustomPlanSpecification : BaseSpecification<Meal>
{
    public MealUserCustomPlanSpecification() : base(x=>!x.IsDeleted && x.IsAvailable)
    {
    }
}