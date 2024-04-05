using System.Linq.Expressions;
using AsparagusN.Data.Entities;

namespace AsparagusN.Specifications;

public class BundleSpecification : BaseSpecification<Bundle>
{
    public BundleSpecification(int duration,int mealsPerDay) : base(x=>x.Duration == duration && x.MealsPerDay == mealsPerDay)
    {
    }
}