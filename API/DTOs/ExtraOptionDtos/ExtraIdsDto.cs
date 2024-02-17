using System.ComponentModel.DataAnnotations;

namespace AsparagusN.DTOs.ExtraOptionDtos;

public class ExtraIdsDto
{
    [Required(ErrorMessage = "ExtraIds must not be null or empty.")]
    [MinLength(1, ErrorMessage = "At least one ExtraId must be provided.")]
    public List<int> ExtraIds { get; set; }
}