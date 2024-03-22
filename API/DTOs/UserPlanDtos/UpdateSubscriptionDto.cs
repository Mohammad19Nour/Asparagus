using System.ComponentModel.DataAnnotations;
using AsparagusN.Enums;

namespace AsparagusN.DTOs.UserPlanDtos;

public class UpdateSubscriptionDto
{
    public PlanTypeEnum PlanType { get; set; }
    [Range(1,30)]
    public int Duration { get; set; }
    [Range(1,30)]
    public int NumberOfMealPerDay { get; set; }
    [Range(0,500)]
    public int NumberOfSnacks { get; set; }
   
    public List<int>? SelectedDrinks { get; set; }
    public List<Item>? SelectedExtras { get; set; }
    public List<Item>? SelectedSalads { get; set; }
    public string? TransactionId { get; set; }
}