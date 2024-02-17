using AsparagusN.DTOs.AllergyDtos;

namespace AsparagusN.DTOs;

public class SubscriptionDto
{
    public decimal Price { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime StartDate { get; set; }
    public int Duration { get; set; }
    public string PlanType { get; set; }
    public int NumberOfMealPerDay { get; set; }
    public int NumberOfSnacks { get; set; }
    public int NumberOfRemainingSnacks { get; set; }
    public string? Notes { get; set; }
    public string DeliveryCity { get; set; }

    public DateTime EndDate{ get; set; }
}