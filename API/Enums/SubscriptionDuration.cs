using System.Runtime.Serialization;

namespace AsparagusN.Enums;

public enum SubscriptionDuration
{
    [EnumMember(Value = "Five Days")]
    FiveDays = 5,
    [EnumMember(Value = "Seven Days")]
    SevenDays = 20,
    [EnumMember(Value = "Twenty Days")]
    FifteenDays = 26,
    [EnumMember(Value = "Twenty Six Days")]
    ThirtyDays = 30
}