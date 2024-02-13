using System.Linq.Expressions;
using AsparagusN.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class BasketSpecification : BaseSpecification<CustomerBasket>
{
    public BasketSpecification(int id) : base(x=>x.Id == id)
    {
        AddInclude(x=>x.Include(y=>y.Items));
    }
}