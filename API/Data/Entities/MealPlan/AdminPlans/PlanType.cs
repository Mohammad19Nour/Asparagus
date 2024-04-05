using System.ComponentModel.DataAnnotations;
using AsparagusN.Enums;

namespace AsparagusN.Data.Entities.MealPlan.AdminPlans;

public class PlanType
{
    [Key]
    public PlanTypeEnum PlanTypeE{ get; set; }
    public int Points { get; set; }
}