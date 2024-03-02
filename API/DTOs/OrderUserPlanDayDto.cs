using AsparagusN.DTOs.UserPlanDtos;

namespace AsparagusN.DTOs;

public class OrderUserPlanDayDto : UserPlanDayDto
{
    public string PhoneNumber { get; set; }
    public string Username { get; set; }
    public string PictureUrl { get; set; }
}