using AsparagusN.DTOs.OrderDtos;
using AsparagusN.DTOs.ReportDtos;
using AsparagusN.Enums;

namespace AsparagusN.Interfaces;

public interface IReportService
{
    Task<List<BranchReportDto>> GenerateBranchesReport(DateTime startDate, DateTime endDate);
    Task<List<UserReportDto>> GenerateUsersReport(DateTime startDate, DateTime endDate);
    Task<SalesDto> GenerateSalesReport(DateTime startDate, DateTime endDate);
    Task<List<PlanReportDto>> GeneratePlansReport(DateTime startDate, DateTime endDate);
    Task<List<DriverReportDto>> GenerateDriversReport(DateTime startDate, DateTime endDate);
    Task<List<OrderReportDto>> GenerateOrdersReport(DateTime startDate, DateTime endDate);
    public Task<List<OrderReportDto>> GeneratePointOrdersReport(DateTime startDate, DateTime endDate);

}