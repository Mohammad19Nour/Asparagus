using System.Runtime.Serialization;

namespace AsparagusN.Enums;

public enum OrderStatus
{
    [EnumMember(Value = "Pending")]
    Pending,
    [EnumMember(Value = "Delivered")]
    Delivered
    
}