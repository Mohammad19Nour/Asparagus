namespace AsparagusN.Data.Entities;

public class CarWorkingDay
{
    public int Id { get; set; }

    public DayOfWeek Day { get; set; }
    public int CarId { get; set; }
    public Car Car { get; set; }
}