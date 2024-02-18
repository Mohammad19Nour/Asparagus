using System.Runtime.Serialization;

namespace AsparagusN.Enums;

public enum OrderStatus
{
    [EnumMember(Value = "Pending")]
    Pending,
    [EnumMember(Value = "Preparing")]
    Preparing,    
    [EnumMember(Value = "Ready")]
    Ready,
    [EnumMember(Value = "Done")]
    Done
    
}