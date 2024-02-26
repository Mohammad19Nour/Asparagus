using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.Enums;

namespace AsparagusN.Data.Entities;

public class Driver
{
    public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; } = true;
        public Zone Zone { get; set; }
        public int ZoneId { get; set; }
        public string PictureUrl { get; set; }
        public Period Period { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    
    public List<Order> Orders{ get; set; }
    public DriverStatus Status { get; set; } = DriverStatus.Idle;
}