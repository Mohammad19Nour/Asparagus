using System.ComponentModel.DataAnnotations;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Enums;

namespace AsparagusN.DTOs.SubscriptionDtos;

public class NewCustomSubscriptionDto
{

    public PlanTypeEnum PlanType { get; set; }
    public DateTime StartDate { get; set; }
    [Range(1,30)]
    public int Duration { get; set; }
    [Range(1,25)]
    public int NumberOfMealPerDay { get; set; }
    [Range(0,500)]
    public int NumberOfSnacks { get; set; }
   
    public List<int>? SelectedDrinks { get; set; }
    public List<Item>? SelectedExtras { get; set; }
    public List<Item>? SelectedSalads { get; set; }
    public List<int>? Allergies { get; set; }
    public string? Notes { get; set; }
    public string DeliveryCity { get; set; }
    [Range(120,int.MaxValue,ErrorMessage = "Protein must be at least 120")]
    public decimal? ProteinPerMeal { get; set; }
    [Range(120,int.MaxValue,ErrorMessage = "Carb must be at least 120")]
    public decimal? CarbPerMeal { get; set; }

    public string? TransactionId { get; set; }
}
