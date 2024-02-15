using System.Runtime.Serialization;

namespace AsparagusN.Enums;

public enum AppCouponType
{
    [EnumMember(Value = "Percent")] Percent,
    [EnumMember(Value = "Fixed Amount")] FixedAmount
}