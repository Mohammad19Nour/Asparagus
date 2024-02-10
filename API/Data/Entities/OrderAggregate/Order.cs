using AsparagusN.Enums;

namespace AsparagusN.Entities.OrderAggregate;

public class Order
{
    public int Id { get; set; }
    public string BuyerEmail { get; set; }
    public IReadOnlyList<OrderItem> Items { get; set; }
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
    public Address ShipToAddress { get; set; }
    public decimal Subtotal { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public decimal GetTotal()
    {
        return Subtotal;
    }

}