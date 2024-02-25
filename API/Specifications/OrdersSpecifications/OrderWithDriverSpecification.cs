using System.Linq.Expressions;
using AsparagusN.Data.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications.OrdersSpecifications;

public class OrderWithDriverSpecification : BaseSpecification<Order>
{
    public OrderWithDriverSpecification(int orderId, int driverId)
        : base(x=>x.DriverId == driverId && orderId == x.Id)
    {
        AddInclude(x=>x.Include(y=>y.Driver));
    }
}