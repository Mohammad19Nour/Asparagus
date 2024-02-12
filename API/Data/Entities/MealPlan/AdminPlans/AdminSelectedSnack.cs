using System.ComponentModel.DataAnnotations.Schema;
using AsparagusN.Entities;
using AsparagusN.Enums;

namespace AsparagusN.Data.Entities.MealPlan.AdminPlans;

public class AdminSelectedSnack
{
    public int Id { get; set; }
    [ForeignKey("SnackId")] public int SnackId { get; set; }
    public Meal Snack { get; set; }
    public PlanTypeEnum PlanTypeEnum { get; set; }
}