namespace AsparagusN.DTOs.AddressDtos;

public class UpdateAddressDto : UpdateLocationDto
{
    public string? BuildingName { get; set; }
    public int? ApartmentNumber { get; set; }
}