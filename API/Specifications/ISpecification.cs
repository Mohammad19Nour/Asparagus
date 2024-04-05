using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace AsparagusN.Specifications;

public interface ISpecification <T>
{
    Expression<Func<T, bool>> Criteria { get; }
    List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> Includes { get; }

}