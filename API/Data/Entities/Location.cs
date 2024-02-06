namespace AsparagusN.Entities;

public class Location
{
    public int Id { get; set; }
    public string City { get; set; }
    public string StreetName { get; set; }
    public decimal Longitude { get; set; }
    public decimal Latitude { get; set; }
}