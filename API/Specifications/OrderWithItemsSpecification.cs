using System.Linq.Expressions;
using AsparagusN.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class OrderWithItemsSpecification : BaseSpecification<Order>
{
    public OrderWithItemsSpecification(string email) : base(x=>x.BuyerEmail == email)
    {
        AddInclude(x=>x.Include(y=>y.Items));
    }
    public OrderWithItemsSpecification(string email,int id) : base(x=>x.Id == id 
                                                                      && email == x.BuyerEmail)
    {
        AddInclude(x=>x.Include(y=>y.Items));
    }
}