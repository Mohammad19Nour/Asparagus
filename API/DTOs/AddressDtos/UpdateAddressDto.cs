namespace AsparagusN.DTOs;

public class UpdateAddressDto : UpdateLocationDto
{
    public string? BuildingName { get; set; }
    public int? ApartmentNumber { get; set; }
}