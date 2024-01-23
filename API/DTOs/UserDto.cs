﻿using AsparagusN.Enums;

namespace AsparagusN.DTOs;

public class UserDto
{
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Token { get; set; }
    public string PictureUrl { get; set; }
    public DateTime Birthday { get; set; }
    public DateTime RegistrationDate { get; set; }
    public Gender Gender { get; set; }
    public bool IsMealPlanMember { get; set; }
    public AddressDto HomeAddress { get; set; }
    public AddressDto WorkAddress { get; set; }
}