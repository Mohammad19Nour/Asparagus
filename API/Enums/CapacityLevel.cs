using System.Runtime.Serialization;

namespace AsparagusN.Enums;

public enum CapacityLevel
{
    [EnumMember(Value = "Small")]
    Small,
    [EnumMember(Value = "Medium")]
    Medium,
    [EnumMember(Value = "Large")]
    Large
}