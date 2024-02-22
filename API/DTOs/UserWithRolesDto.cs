namespace AsparagusN.DTOs;

public class UserWithRolesDto
{
    public UserWithRolesDto(int id, string email, string fullName, List<string> roles)
    {
        Id = id;
        Email = email;
        FullName = fullName;
        Roles = roles;
    }

    public int Id { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public List<string> Roles { get; set; } 
}