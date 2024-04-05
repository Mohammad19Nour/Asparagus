namespace AsparagusN.Data.Entities;

public class Address
{
    public int Id { get; set; }
    public string City { get; set; } = "";
    public string StreetName { get; set; } = "";
    public string BuildingName { get; set; } = "";
    public int ApartmentNumber { get; set; } = 0;
    public decimal Longitude { get; set; } = 0;
    public decimal Latitude { get; set; } = 0;
}