namespace AsparagusN.DTOs.EmployeeDtos;

public class EmployeeDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public Dictionary<string,bool> Roles { get; set; }
}