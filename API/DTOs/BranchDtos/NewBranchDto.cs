using AsparagusN.DTOs.AddressDtos;

namespace AsparagusN.DTOs.BranchDtos;

public class NewBranchDto
{
    public string NameAR { get; set; }
    public string NameEN { get; set; }
    public LocationDto Address { get; set; }
}