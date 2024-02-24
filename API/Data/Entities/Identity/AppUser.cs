using AsparagusN.Enums;
using AsparagusN.Extensions;
using Microsoft.AspNetCore.Identity;

namespace AsparagusN.Data.Entities.Identity;

public class AppUser : IdentityUser<int>
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; }
    public string PhoneNumber { get; set; } = "";
    public string PictureUrl { get; set; } = "";
    public DateTime Birthday { get; set; } = DateTime.UtcNow;
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    public Gender Gender { get; set; } = Gender.Male;
    public bool IsMealPlanMember { get; set; } = false;
    public int LoyaltyPoints { get; set; }
    public Address HomeAddress { get; set; } = new();
    public Address WorkAddress { get; set; } = new();
    public int HomeAddressId { get; set; }
    public int WorkAddressId { get; set; }
    public ICollection<AppUserRole> UserRoles { get; set; }
    public bool IsNormalUser { get; set; } = true;

    public int GetAge()
    {
        return Birthday.NumberOfYears();
    }
}