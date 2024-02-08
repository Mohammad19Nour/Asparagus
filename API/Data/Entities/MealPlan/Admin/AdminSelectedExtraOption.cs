using AsparagusN.Entities;
using AsparagusN.Entities.MealPlan;
using AsparagusN.Enums;

namespace AsparagusN.Data.Entities.MealPlan.Admin;

public class AdminSelectedExtraOption
{
    public int Id { get; set; }
    public ExtraOption ExtraOption { get; set; }
    public int ExtraOptionId { get; set; }
    public PlanType PlanType { get; set; }
}