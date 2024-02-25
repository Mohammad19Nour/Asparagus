using AsparagusN.Enums;

namespace AsparagusN.Data.Entities.OrderAggregate;

public class Order
{
    public int Id { get; set; }
    public string BuyerEmail { get; set; }
    public string BuyerPhoneNumber { get; set; }
    public ICollection<OrderItem> Items { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public Address ShipToAddress { get; set; }
    public Branch Branch { get; set; }
    public int BranchId { get; set; }
    public decimal Subtotal { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public PaymentType PaymentType { get; set; }
    public int PointsPrice { get; set; }
    public string? BillId { get; set; }
    public Driver? Driver { get; set; }
    public int? DriverId { get; set; }
    public int  GainedPoints { get; set; }
    public decimal CouponValue { get; set; } = 0;
    public int? Priority { get; set; }

    public decimal GetTotal()
    {
        return decimal.Max(0 , Subtotal - CouponValue);
    }
    

}