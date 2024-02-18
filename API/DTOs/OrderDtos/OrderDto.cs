using AsparagusN.DTOs.AddressDtos;
using AsparagusN.Enums;

namespace AsparagusN.DTOs.OrderDtos;

public class OrderDto
{
    public int Id { get; set; }
    public string BuyerEmail { get; set; }
    public ICollection<OrderItemDto> Items { get; set; }
    public DateTimeOffset OrderDate { get; set; }
    public AddressDto ShipToAddress { get; set; }
    public decimal Subtotal { get; set; }
    public string Status { get; set; }
    public string PaymentType { get; set; }
    public string BranchNameAR { get; set; }
    public string BranchNameEN { get; set; }
    public int PointsPrice { get; set; }
}