namespace AsparagusN.Data.Entities;

public class Car
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsAvailable { get; set; } = true;
    public List<Booking> Bookings { get; set; } = new List<Booking>();
    public DateTime WorkingStartHour { get; set; }
    public DateTime WorkingEndHour { get; set; }
    public List<DayOfWeek> WorkingDays { get; set; }
}