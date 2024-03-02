namespace AsparagusN.DTOs.CarDtos;

public class CatInfoDto
{
    public string Name { get; set; } = "Car 1";
    public bool IsAvailable { get; set; } = true;
  //  public List<Booking> Bookings { get; set; } = new List<Booking>();
    public TimeSpan WorkingStartHour { get; set; }
    public TimeSpan WorkingEndHour { get; set; }
    //public List<CarWorkingDay> WorkingDays { get; set; } = new List<CarWorkingDay>();
}