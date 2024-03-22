using AsparagusN.DTOs;
using AsparagusN.Enums;
using AsparagusN.Interfaces;

namespace AsparagusN.Services;

public class PlanRecommendationService : IPlanRecommendationService
{
    public PlanTypeEnum GetRecommendedPlan(RecommendationInput input)
    {
        var bmr = GetBMR(input);
        var tdee = GetTDEE(bmr, input.ActivityLevel);
        return input.TargetPlan;
    }

    private double GetBMR(RecommendationInput input)
    {
        var bmr = 10 * input.WeightKg + 6.25 * input.HeightCm - 5 * input.Age;
        if (input.Gender == Gender.Male) bmr += 5;
        else bmr -= 161;
        return bmr;
    }

    private double GetTDEE(double bmr, ActivityLevel activityLevel)
    {
        var tdee = activityLevel switch
        {
            ActivityLevel.Sedentary => bmr,
            ActivityLevel.LightlyActive => bmr * 1.375,
            ActivityLevel.ModeratelyActive => bmr * 1.55,
            ActivityLevel.VeryActive => bmr * 1.725,
            ActivityLevel.ExtraActive => bmr * 1.9,
            _ => throw new ArgumentOutOfRangeException(nameof(activityLevel), activityLevel, null)
        };

        return tdee;
    }
}