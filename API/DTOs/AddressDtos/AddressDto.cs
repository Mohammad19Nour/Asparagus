namespace AsparagusN.DTOs;

public class AddressDto : LocationDto
{
    public string BuildingName { get; set; }
    public int ApartmentNumber { get; set; }
}