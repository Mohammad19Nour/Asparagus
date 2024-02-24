using System.ComponentModel.DataAnnotations;

namespace AsparagusN.DTOs.AccountDtos;

public class LoginDto
{
    [Required,EmailAddress] public string Email { get; set; }
    [Required] public string Password { get; set; }
}