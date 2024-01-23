using AsparagusN.Enums;
using AsparagusN.Extensions;

namespace AsparagusN.Entities;

public class UserAddress
{
    public int Id { get; set; }
    public string City { get; set; }
    public string StreetName { get; set; }
    public string BuildingName { get; set; }
    public int ApartmentNumber { get; set; }
}