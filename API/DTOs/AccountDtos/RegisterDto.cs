using System.ComponentModel.DataAnnotations;
using AsparagusN.Enums;

namespace AsparagusN.DTOs;


public class RegisterDto
{
    [Required] public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmedPassword { get; set; }
    public Gender Gender { get; set; }
    
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime Birthday { get; set; }
    public AddressDto HomeAddress { get; set; }
    public AddressDto WorkAddress { get; set; }
}