using AsparagusN.DTOs.AddressDtos;
using AsparagusN.DTOs.DriverDtos;
using AsparagusN.Enums;

namespace AsparagusN.DTOs.OrderDtos;

public class OrderDto
{
    public int Id { get; set; }
    public string BuyerEmail { get; set; }
    public string BuyerPhoneNumber { get; set; }
    public ICollection<OrderItemDto> Items { get; set; }
    public DateTimeOffset OrderDate { get; set; }
    public AddressDto ShipToAddress { get; set; }
    public decimal Subtotal { get; set; }
    public string Status { get; set; }
    public string PaymentType { get; set; }
    public string BranchNameAR { get; set; }
    public string BranchNameEN { get; set; }
    public int PointsPrice { get; set; }
    public int? Priority { get; set; }
    public OrderDriverDto Driver { get; set; }
    public int  GainedPoints { get; set; }
    public decimal CouponValue { get; set; } = 0;
    public string? BillId { get; set; }
    public decimal Total { get; set; }
}