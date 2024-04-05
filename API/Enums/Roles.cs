using System.Runtime.Serialization;

namespace AsparagusN.Enums;

public enum Roles
{
    Admin,
    [EnumMember(Value = "Driver")]
    Driver,
    Cashier,
    User,
    Employeee
}