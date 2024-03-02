using System.Linq.Expressions;
using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications.OrdersSpecifications;

public class OrdersInBranchSpecification : BaseSpecification<Order>
{
    public OrdersInBranchSpecification(int branchId, OrderStatus status)
        : base(x => x.BranchId == branchId && x.Status == status)
    {
        AddInclude(c=>c.Include(x=>x.Items));
    }
}