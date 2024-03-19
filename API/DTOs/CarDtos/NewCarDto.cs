using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AsparagusN.DTOs.CarDtos;

public class NewCarDto
{
    [RegularExpression(@"^(?:[01]\d|2[0-3]):(?:[0-5]\d)$")]
    public string? WorkingStartHour { get; set; }

    [RegularExpression(@"^(?:[01]\d|2[0-3]):(?:[0-5]\d)$")]
    public string? WorkingEndHour { get; set; }
    [MinLength(7,ErrorMessage = "WorkingDays list must have a length of 7 "),MaxLength(7, ErrorMessage = "WorkingDays list must have a length of 7 representing all days of the week.")]

    public List<DayOfWeek> WorkingDays { get; set; }

    public string City { get; set; }
}