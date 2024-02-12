using System.ComponentModel.DataAnnotations;

namespace AsparagusN.DTOs.UserPlanDtos;

public class UpdateSubscriptionDto
{
    [Range(1,int.MaxValue)]
    public int Duration { get; set; }
    [Range(1,int.MaxValue)]
    public int NumberOfMealPerDay { get; set; }
    [Range(0,int.MaxValue)]
    public int NumberOfSnacks { get; set; }
   
    public List<Item>? SelectedDrinks { get; set; }
    public List<Item>? SelectedExtras { get; set; }
}