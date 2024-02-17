namespace AsparagusN.Data.Entities;

public class Branch
{
    public int Id { get; set; }
    public string NameEN { get; set; }
    public string NameAR { get; set; }
    public Location Address { get; set; }
    public int AddressId { get; set; }
}