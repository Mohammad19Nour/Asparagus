namespace AsparagusN.Data.Entities;

public class Employee
{
    public int Id { get; set; }
    public string FullName { get; set; }

    public string Email { get; set; }
    public string Password { get; set; }
    public string PictureUrl { get; set; }
    public bool IsActive { get; set; }
}