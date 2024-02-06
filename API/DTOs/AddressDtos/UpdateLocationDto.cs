namespace AsparagusN.DTOs;

public class UpdateLocationDto
{
    public string? City { get; set; }
    public string? StreetName { get; set; }
   
    public decimal? Longitude { get; set; }
    public decimal? Latitude { get; set; }
}