using System.ComponentModel.DataAnnotations;

namespace AsparagusN.DTOs.SnackDtos;

public class UserUpdateSnackDto
{
    public int UserOldSnackId { get; set; }
    public int? AdminNewSnackId { get; set; }
    [Range(1,int.MaxValue,ErrorMessage = "quantity should be positive")]
    public int Quantity { get; set; }
}