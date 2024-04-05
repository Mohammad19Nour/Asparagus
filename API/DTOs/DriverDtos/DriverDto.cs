using AsparagusN.DTOs.ZoneDtos;

namespace AsparagusN.DTOs.DriverDtos;

public class DriverDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public string Period { get; set; }
    public ZoneDto Zone { get; set; }
    public string PictureUrl { get; set; }
    public DateTime RegistrationDate { get; set; }
}