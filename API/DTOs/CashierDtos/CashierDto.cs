using AsparagusN.DTOs.BranchDtos;

namespace AsparagusN.DTOs.CashierDtos;

public class CashierDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string PictureUrl { get; set; }
    public bool IsActive { get; set; }
    public string Period { get; set; }
    public BranchDto Branch { get; set; }
}