using System.ComponentModel.DataAnnotations;
using AsparagusN.Enums;

namespace AsparagusN.Data;

public class PlanPrice
{
    public int Id { get; set; }
    public SubscriptionDuration Duration { get; set; }

    [Range(0,3)]
    public int NumberOfMealsPerDay { get; set; }

    public decimal Price { get; set; }
}