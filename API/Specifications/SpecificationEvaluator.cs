using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class SpecificationEvaluator<TEntity> where TEntity : class
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
    {
        var query = inputQuery;

        query = query.Where(spec.Criteria);

        query = spec.Includes.Aggregate(query,(current,include)
                => include(current))
            ;

        return query;
    }
}