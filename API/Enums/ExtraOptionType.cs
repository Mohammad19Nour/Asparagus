using System.Runtime.Serialization;

namespace AsparagusN.Enums;

public enum ExtraOptionType
{
    [EnumMember(Value = "Salad")]
    Salad,
    [EnumMember(Value = "Nuts")]
    Nuts,
    [EnumMember(Value = "Sauce")]
    Sauce
}