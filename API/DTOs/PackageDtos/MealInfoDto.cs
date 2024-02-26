namespace AsparagusN.DTOs.PackageDtos;

public class MealInfoDto
{
    public string NameEN { get; set; }
    public string NameAR { get; set; }
    public string DescriptionEN { get; set; }
    public string DescriptionAR { get; set; }
    public string CarbNameEN { get; set; } = "This meal doesn't have carb";
    public string CarbNameAR { get; set; } = "لا تحتوي هذه الوجبة على كارب";
    public decimal Calories{ get; set; }
    public decimal Fibers{ get; set; }
    public decimal Fats{ get; set; }
    public decimal Carbs{ get; set; }
    public decimal Protein{ get; set; }
}