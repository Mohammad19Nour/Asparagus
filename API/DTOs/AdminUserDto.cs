using AsparagusN.DTOs.AddressDtos;

namespace AsparagusN.DTOs;

public class AdminUserDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string PictureUrl { get; set; }
    public DateTime Birthday { get; set; } 
    public DateTime RegistrationDate { get; set; }
    public string Gender { get; set; }
    public bool IsMealPlanMember { get; set; }
    public int LoyaltyPoints { get; set; }
    public AddressDto HomeAddress { get; set; }
    public AddressDto WorkAddress { get; set; }
    public bool IsNormalUser { get; set; } = true;

    public int GetAge;
}