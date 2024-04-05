using AsparagusN.DTOs.AddressDtos;

namespace AsparagusN.DTOs.BranchDtos;

public class UpdateBranchDto
{
    public string? NameAR { get; set; }
    public string? NameEN { get; set; }
    public UpdateLocationDto? UpdatedAddress { get; set; }
}