namespace AsparagusN.DTOs.DriverDtos;

public class NewDriverDto :LoginDto
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public int ZoneId { get; set; }
}