using AsparagusN.DTOs.AddressDtos;

namespace AsparagusN.DTOs.UserDtos;

public class UserInfoDto
{
    public string Email { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime Birthday { get; set; }
    public AddressDto HomeAddress { get; set; }
    public AddressDto WorkAddress { get; set; }
    public string PictureUrl { get; set; }
    public string Gender { get; set; }
}