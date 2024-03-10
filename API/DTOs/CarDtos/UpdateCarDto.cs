using System.ComponentModel.DataAnnotations;
using AsparagusN.Helpers;
using Newtonsoft.Json;

namespace AsparagusN.DTOs.CarDtos;

public class UpdateCarDto
{
        [RegularExpression(@"^(0?[0-9]|1[0-9]|2[0-3])(:[0-5]?[0-9]){1,2}$")]
    public string? WorkingStartHour { get; set; }

    [RegularExpression(@"^(0?[0-9]|1[0-9]|2[0-3])(:[0-5]?[0-9]){1,2}$")]
    public string? WorkingEndHour { get; set; }

    [MinLength(7, ErrorMessage = "WorkingDays list must have a length of 7 "),
     MaxLength(7, ErrorMessage = "WorkingDays list must have a length of 7 representing all days of the week.")]
    public List<bool> WorkingDays { get; set; } = new List<bool>();
}