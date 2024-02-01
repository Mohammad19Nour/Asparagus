﻿using AsparagusN.Enums;
using AsparagusN.Extensions;
using Microsoft.AspNetCore.Identity;

namespace AsparagusN.Entities.Identity;

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
    public Address? HomeAddress { get; set; }
    public Address? WorkAddress { get; set; }
    public ICollection<AppUserRole> UserRoles { get; set; }

    public int GetAge()
    {
        return Birthday.NumberOfYears();
    }
    
}