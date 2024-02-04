namespace AsparagusN.Entities;

public class Driver
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; } = true;
    public Zone Zone { get; set; }
    public int ZoneId { get; set; }
}