using AsparagusN.DTOs.AddressDtos;
using AsparagusN.Enums;

namespace AsparagusN.DTOs.UserDtos;

public class UpdateUserInfoDto
{
    public string? FullName { get; set; }
    public Gender? Gender { get; set; }
    public IFormFile? Image { get; set; }
}