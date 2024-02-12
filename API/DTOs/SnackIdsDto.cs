using System.ComponentModel.DataAnnotations;

namespace AsparagusN.DTOs;

public class SnackIdsDto
{
    [Required(ErrorMessage = "ExtraIds must not be null or empty.")]
    [MinLength(1, ErrorMessage = "At least one SnackId must be provided.")]
    public List<int> SnackIds { get; set; }
}