using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;

namespace AsparagusN.Data;

public class Booking
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public Car Car { get; set; }
    public AppUser User { get; set; }
    public int UserId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}