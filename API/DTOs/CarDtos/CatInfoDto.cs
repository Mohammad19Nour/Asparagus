namespace AsparagusN.DTOs.CarDtos;

public class CatInfoDto
{
    public string Name { get; set; } = "Car 1";
    public bool IsAvailable { get; set; } = true;
    public string WorkingStartHour { get; set; }
    public string WorkingEndHour { get; set; }
    public List<BookingDto> Bookings { get; set; } = new List<BookingDto>();
    public List<bool> WorkingDays { get; set; } = new List<bool>();
}