using AsparagusN.DTOs.AddressDtos;

namespace AsparagusN.DTOs.BranchDtos;

public class BranchDto
{
    public int Id { get; set; }
    public string NameAR { get; set; }
    public string NameEN { get; set; }
    public LocationDto Address { get; set; }
    
}