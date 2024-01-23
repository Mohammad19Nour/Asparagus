using System.ComponentModel.DataAnnotations;

namespace AsparagusN.DTOs;


public class RegisterDto
{
    [Required] public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmedPassword { get; set; }
}