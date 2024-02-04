namespace AsparagusN.Entities;

public class Zone
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Driver> Drivers { get; set; }
}