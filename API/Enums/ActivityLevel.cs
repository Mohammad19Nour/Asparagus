using System.Runtime.Serialization;

namespace AsparagusN.Enums;

public enum ActivityLevel
{
    [EnumMember(Value = "Sedentary")]
    Sedentary,

    [EnumMember(Value = "LightlyActive")]
    LightlyActive,

    [EnumMember(Value = "ModeratelyActive")]
    ModeratelyActive ,

    [EnumMember(Value = "VeryActive")]
    VeryActive,

    [EnumMember(Value = "ExtraActive")]
    ExtraActive
}