using System.ComponentModel.DataAnnotations;
using AsparagusN.Enums;

namespace AsparagusN.DTOs.UserPlanDtos;

public class NewSubscriptionDto
{
    public PlanTypeEnum PlanType { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Today;
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
}

public class Item
{
    public int Id { get; set; }
    [Range(1,int.MaxValue)]
    public int Weight { get; set; }
}