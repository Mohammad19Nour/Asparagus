using AsparagusN.Enums;

namespace AsparagusN.DTOs.CouponDtos;

public class CouponDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public decimal Value { get; set; }
    public string Type { get; set; }
}