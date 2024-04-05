using AsparagusN.Enums;

namespace AsparagusN.DTOs.UserPlanDtos;

public class UserPlanInfoDto
{
    public PlanTypeEnum PlanType { get; set; }
    public List<int>? Allergies { get; set; }
    public string? Notes { get; set; }
    public string? DeliveryCity { get; set; }
}