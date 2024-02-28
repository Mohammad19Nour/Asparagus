using System.ComponentModel.DataAnnotations;
using AsparagusN.Helpers;
using Newtonsoft.Json;

namespace AsparagusN.DTOs.CarDtos;

public class UpdateCarDto
{
    [JsonConverter(typeof(TimeSpanConverter))]
    public TimeSpan? WorkingStartHour { get; set; }
    [JsonConverter(typeof(TimeSpanConverter))]      
    public TimeSpan? WorkingEndHour { get; set; }
    [MinLength(7,ErrorMessage = "WorkingDays list must have a length of 7 "),MaxLength(7, ErrorMessage = "WorkingDays list must have a length of 7 representing all days of the week.")]
    public List<int> WorkingDays { get; set; } = new List<int>();
}