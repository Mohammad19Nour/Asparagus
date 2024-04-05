using AsparagusN.Enums;

namespace AsparagusN.Data.Entities;

public class AppCoupon
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public decimal Value { get; set; }
    public AppCouponType Type { get; set; }
}