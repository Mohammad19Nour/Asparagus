using System.Linq.Expressions;
using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.Enums;

namespace AsparagusN.Specifications.OrdersSpecifications;

public class DoneOrdersSpecification : BaseSpecification<Order>
{
    public DoneOrdersSpecification(DateTime startDate, DateTime endDate) : base( c=> c.PaymentType != PaymentType.Points &&
        c.OrderDate.Date >= startDate && c.OrderDate.Date <= endDate && c.Status == OrderStatus.Delivered)
    {
    }
}