using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace AsparagusN.Specifications;

public class BaseSpecification<T> : ISpecification<T>
{
    public BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    public Expression<Func<T, bool>> Criteria { get; }
    public  List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> Includes { get; } = new List<Func<IQueryable<T>, IIncludableQueryable<T, object>>>();
    public void AddInclude(Func<IQueryable<T>, IIncludableQueryable<T, object>>exp)
    {
      Includes.Add(exp);   
    }
   
}