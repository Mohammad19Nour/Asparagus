using System.Linq.Expressions;
using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications.OrdersSpecifications;

public class DoneOrdersWithBranchSpecification : BaseSpecification<Order>
{
    public DoneOrdersWithBranchSpecification(DateTime start, DateTime end)
        : base(x=>(x.PaymentType == PaymentType.Cash || x.PaymentType == PaymentType.Card) &&x.Status == OrderStatus.Done && x.OrderDate >= start && x.OrderDate <= end)
    {
        AddInclude(y=>y.Include(c=>c.Branch));
    }
    
    public DoneOrdersWithBranchSpecification(int branchId,DateTime start, DateTime end)
        : base(x=> (x.PaymentType == PaymentType.Cash || x.PaymentType == PaymentType.Card) && x.BranchId == branchId && x.Status == OrderStatus.Done && x.OrderDate >= start && x.OrderDate <= end)
    {
        AddInclude(y=>y.Include(c=>c.Branch));
    }
}