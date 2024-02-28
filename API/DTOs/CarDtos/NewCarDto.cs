using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AsparagusN.DTOs.CarDtos;

public class NewCarDto
{
    public TimeSpan WorkingStartHour { get; set; }
    public TimeSpan WorkingEndHour { get; set; }
    [MinLength(7,ErrorMessage = "WorkingDays list must have a length of 7 "),MaxLength(7, ErrorMessage = "WorkingDays list must have a length of 7 representing all days of the week.")]

    public List<DayOfWeek> WorkingDays { get; set; }
}