using System.ComponentModel.DataAnnotations;
using AsparagusN.Enums;

namespace AsparagusN.DTOs;

public class RegisterDto
{
    [Required(ErrorMessage = "Email required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password required")]
    public string Password { get; set; }

    [Required(ErrorMessage = "co pass required")]
    public string ConfirmedPassword { get; set; }

    public Gender Gender { get; set; }

    [Required(ErrorMessage = "Full name required")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "number required")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "birthday required")]
    public DateTime Birthday { get; set; }

    public AddressDto HomeAddress { get; set; }
    public AddressDto WorkAddress { get; set; }
    public IFormFile Image { get; set; }
}