using System.Linq.Expressions;
using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.Enums;

namespace AsparagusN.Specifications.OrdersSpecifications;

public class DoneOrdersWithoutPointsPaymentSpecification : BaseSpecification<Order>
{
    public DoneOrdersWithoutPointsPaymentSpecification()
        : base(x => x.Status == OrderStatus.Delivered && x.PaymentType != PaymentType.Points)
    {
    }

    public DoneOrdersWithoutPointsPaymentSpecification(int month, int year)
        : base(x =>
            x.OrderDate.Date.Month == month && x.OrderDate.Date.Year == year &&
            x.Status == OrderStatus.Delivered && x.PaymentType != PaymentType.Points)
    {
    }

    public DoneOrdersWithoutPointsPaymentSpecification(DateTime startDate, DateTime endDate)
        : base(x =>
            x.OrderDate.Date >= startDate && x.OrderDate.Date <= endDate &&
            x.Status == OrderStatus.Delivered && x.PaymentType != PaymentType.Points)
    {
    }
}