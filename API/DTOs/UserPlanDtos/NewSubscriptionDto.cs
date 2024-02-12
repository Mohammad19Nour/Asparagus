using System.ComponentModel.DataAnnotations;
using AsparagusN.Enums;

namespace AsparagusN.DTOs.UserPlanDtos;

public class NewSubscriptionDto
{
    public PlanTypeEnum PlanType { get; set; }
    public DateTime StartDate { get; set; }
   [Range(1,35)]
    public int Duration { get; set; }
    [Range(1,int.MaxValue)]
    public int NumberOfMealPerDay { get; set; }
    [Range(0,int.MaxValue)]
    public int NumberOfSnacks { get; set; }
   
    public List<Item>? SelectedDrinks { get; set; }
    public List<Item>? SelectedExtras { get; set; }
}

public class Item
{
    public int Id { get; set; }
    [Range(1,int.MaxValue)]
    public int Quantity { get; set; }
}