using AsparagusN.Enums;

namespace AsparagusN.DTOs;

public class RecommendationInput
{
    public int Age { get; set; }
    public double HeightCm { get; set; }
    public double WeightKg { get; set; }
    public Gender Gender { get; set; }
    public ActivityLevel ActivityLevel { get; set; }
    public PlanTypeEnum TargetPlan { get; set; }
}