using AsparagusN.DTOs;
using AsparagusN.Enums;

namespace AsparagusN.Interfaces;

public interface IPlanRecommendationService
{
    PlanTypeEnum GetRecommendedPlan(RecommendationInput input);
}