namespace AsparagusN.DTOs.CarbDtos;

public class CarbDto
{
    public int Id { get; set; }
    public string NameEN { get; set; } 
    public string NameAR { get; set; } 
    public string ExtraInfo { get; set; }
    public decimal Weight { get; set; }
    public decimal Price { get; set; }
    public decimal Protein { get; set; }
    public decimal Carb { get; set; }
    public decimal Fat { get; set; }
    public decimal Fiber { get; set; }
}