using System.ComponentModel.DataAnnotations;

namespace AsparagusN.DTOs.EmployeeDtos;

public class NewEmployeeDto
{
    public string FullName { get; set; }

    [EmailAddress] public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
}