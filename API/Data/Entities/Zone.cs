namespace AsparagusN.Data.Entities;

public class Zone
{
    public int Id { get; set; }
    public string NameAR { get; set; }
    public string NameEN { get; set; }
    public ICollection<Driver> Drivers { get; set; }
}