using System.Runtime.Serialization;
using AsparagusN.Data.Entities;
using AsparagusN.DTOs.AddressDtos;
using AsparagusN.Enums;

namespace AsparagusN.DTOs.OrderDtos;

public class NewOrderInfoDto
{
    public AddressDto ShipToAddress { get; set; }
    public int BranchId { get; set; }
    public PaymentType PaymentType { get; set; }
    public string? CouponCode { get; set; }
    public string? BillId { get; set; }
}