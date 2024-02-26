using AsparagusN.DTOs.AllergyDtos;

namespace AsparagusN.DTOs.PackageDtos;

public class PackageDto
{
    public CustomerInfoDto CustomerInfo { get; set; }
    public List<AllergyDto> Allergies;
    public List<MealInfoDto> Meals;
    public List<DrinkInfoDto> Drinks { get; set; }
    public List<SaladInfoDto> Salads { get; set; }
    public List<NutsInfoDto>Nuts { get; set; }
    public List<SnackInfoDto> Snacks;
    public bool IsCustomerInfoPrinted { get; set; }
    public bool IsMealsInfoPrinted { get; set; }
    public string? Notes { get; set; }
    public string PlanType { get; set; }
}

