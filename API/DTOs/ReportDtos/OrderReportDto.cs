namespace AsparagusN.DTOs.OrderDtos;

public class OrderReportDto
{
    public int Id { get; set; }
    public string BuyerPhoneNumber { get; set; }
    public string BuyerEmail { get; set; }
    public decimal Total { get; set; }
    public string PaymentType { get; set; }
    public string? BillId { get; set; } = "Not found";
    public decimal CouponValue { get; set; } = 0;
}