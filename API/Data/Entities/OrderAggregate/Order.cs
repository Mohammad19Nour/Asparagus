using AsparagusN.Enums;

namespace AsparagusN.Data.Entities.OrderAggregate;

public class Order
{
    public int Id { get; set; }
    public string BuyerEmail { get; set; }
    public ICollection<OrderItem> Items { get; set; }
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
    public Address ShipToAddress { get; set; }
    public Branch Branch { get; set; }
    public int BranchId { get; set; }
    public decimal Subtotal { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public PaymentType PaymentType { get; set; }
    public int PointsPrice { get; set; }
    public string? BillId { get; set; }

    public decimal GetTotal()
    {
        return Subtotal;
    }

}