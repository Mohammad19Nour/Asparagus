using AsparagusN.Entities.Identity;
using AsparagusN.Enums;
using AsparagusN.Extensions;

namespace AsparagusN.Entities;

public class Address
{
    public int Id { get; set; }
    public string City { get; set; }
    public string StreetName { get; set; }
    public string BuildingName { get; set; }
    public int ApartmentNumber { get; set; }
    public decimal Longitude { get; set; }
    public decimal Latitude { get; set; }
   
}