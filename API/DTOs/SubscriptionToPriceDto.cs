using System.ComponentModel.DataAnnotations;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Enums;

namespace AsparagusN.DTOs;

public class SubscriptionToPriceDto
{
    public PlanTypeEnum PlanType { get; set; }
    public DateTime StartDate { get; set; }
    [Range(1,30)]
    public int Duration { get; set; }
    [Range(1,100)]
    public int NumberOfMealPerDay { get; set; }
    [Range(0,int.MaxValue)]
    public int NumberOfSnacks { get; set; }
   
    public List<int>? SelectedDrinks { get; set; }
    public List<Item>? SelectedExtras { get; set; }
    public List<Item>? SelectedSalads { get; set; }
    public List<int>? Allergies { get; set; }
    public string? Notes { get; set; }
    public string DeliveryCity { get; set; }
}