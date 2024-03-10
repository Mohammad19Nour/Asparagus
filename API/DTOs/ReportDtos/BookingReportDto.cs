namespace AsparagusN.DTOs.ReportDtos;

public class BookingReportDto
{
    
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; }
    public string PhoneNumber { get; set; } = "";
    public string Gender { get; set; }
    public int Age { get; set; }
}