namespace AsparagusN.DTOs.ReportDtos;

public class UserReportDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Gender { get; set; }
    public int NumberOfPlans { get; set; }
    public int NumberOfOrders { get; set; }
    public int Age { get; set; }
    public DateTime RegistrationDate { get; set; }
    
}