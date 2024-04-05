using System.Runtime.Serialization;

namespace AsparagusN.Enums;

public enum ReportType
{
    [EnumMember(Value = "PDF")]
    PDF,
    [EnumMember(Value = "Excel")]
    Excel
}