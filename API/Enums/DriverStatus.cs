using System.Runtime.Serialization;

namespace AsparagusN.Enums;

public enum DriverStatus
{
    
    [EnumMember(Value = "Idle")]
    Idle,

    [EnumMember(Value = "Delivering")]
    Delivering,

    [EnumMember(Value = "OnBreak")]
    OnBreak
}