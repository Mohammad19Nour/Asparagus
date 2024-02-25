namespace AsparagusN.DTOs.ReportDtos;

public class SalesDto
{
    public decimal TotalOrderSales { get; set; }
    public int NumberOfOrders { get; set; }
    
    public decimal TotalPlanSales { get; set; }
    public int NumberOfPlans { get; set; }
}