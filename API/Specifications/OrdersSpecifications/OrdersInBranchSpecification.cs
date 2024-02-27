using System.Linq.Expressions;
using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.Enums;

namespace AsparagusN.Specifications.OrdersSpecifications;

public class OrdersInBranchSpecification : BaseSpecification<Order>
{
    public OrdersInBranchSpecification(int branchId, OrderStatus status)
        : base(x => x.BranchId == branchId && x.Status == status)
    {
    }
}