using System.Linq.Expressions;
using AsparagusN.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class FAQSpecification : BaseSpecification<FAQ>
{
    public FAQSpecification(int id) : base(x => x.Id == id)
    {
        AddInclude(x => x.Include(y => y.FAQChildern).ThenInclude(c => c.FAQChildern));
    }
    public FAQSpecification() : base(x =>x.ParentFAQId == null)
    {
        AddInclude(x => x.Include(y => y.FAQChildern).ThenInclude(c => c.FAQChildern));
    }
}