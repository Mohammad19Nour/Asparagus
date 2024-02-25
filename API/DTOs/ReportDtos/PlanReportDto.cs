namespace AsparagusN.DTOs.ReportDtos;

public class PlanReportDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; }
    public int Duration { get; set; }
    public string PlanType { get; set; }
    public int NumberOfMealPerDay { get; set; }
    public int NumberOfSnacks { get; set; }
    public string? Notes { get; set; }

    public DateTime EndDate;
}