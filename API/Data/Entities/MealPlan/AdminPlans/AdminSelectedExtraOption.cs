using AsparagusN.Enums;

namespace AsparagusN.Data.Entities.MealPlan.AdminPlans;

public class AdminSelectedExtraOption
{
    public int Id { get; set; }
    public ExtraOption ExtraOption { get; set; }
    public int ExtraOptionId { get; set; }
    public PlanTypeEnum PlanTypeEnum { get; set; }
}