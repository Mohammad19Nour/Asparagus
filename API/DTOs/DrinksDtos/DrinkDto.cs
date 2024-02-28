namespace AsparagusN.DTOs.DrinksDtos;

public class DrinkDto
{
    public int Id { get; set; }
    public string NameArabic { get; set; }
    public string NameEnglish { get; set; }
    public decimal Price { get; set; }
    public string PictureUrl { get; set; }
    public string Volume { get; set; }
    public decimal Protein{ get; set; }
    public decimal Carb{ get; set; }
    public decimal Fat{ get; set; }
    public decimal Fiber{ get; set; }
    public decimal Calories { get; set; }
}