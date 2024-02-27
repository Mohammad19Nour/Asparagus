using AsparagusN.Enums;

namespace AsparagusN.Data.Entities;

public class Cashier
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsActive { get; set; } = true;
    public Branch Branch { get; set; }
    public int BranchId { get; set; }
    public string PictureUrl { get; set; }
    public Period Period { get; set; }
}