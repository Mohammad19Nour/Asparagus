using System.Runtime.Serialization;

namespace AsparagusN.Enums;

public enum PaymentType
{
    [EnumMember(Value = "Cash")]
    Cash,
    [EnumMember(Value = "Card")]
    Card,
    [EnumMember(Value = "Points")]
    Points
}