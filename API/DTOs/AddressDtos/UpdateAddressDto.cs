namespace AsparagusN.DTOs;

public class UpdateAddressDto
{
    public string? City { get; set; }
    public string? StreetName { get; set; }
    public string? BuildingName { get; set; }
    public int? ApartmentNumber { get; set; }
    public decimal? Longitude { get; set; }
    public decimal? Latitude { get; set; }
}