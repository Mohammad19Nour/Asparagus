using AsparagusN.DTOs.OrderDtos;
using AsparagusN.DTOs.ReportDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard;

[Authorize(Roles = nameof(DashboardRoles.Export) + "," + nameof(Roles.Admin))]
public class ReportsController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IReportService _reportService;

    public ReportsController(IUnitOfWork unitOfWork, IMapper mapper, IReportService reportService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _reportService = reportService;
    }

    [HttpGet("sales")]
    public async Task<ActionResult<SalesDto>> GetSalesReport(DateTime? start, DateTime? end)
    {
        start ??= DateTime.MinValue;
        end ??= DateTime.Today;

        var result = await _reportService.GenerateSalesReport(start.Value, end.Value);
        return Ok(new ApiOkResponse<SalesDto>(result));
    }

    [HttpGet("users")]
    public async Task<ActionResult<List<UserReportDto>>> GetUsersReport(DateTime? start, DateTime? end)
    {
        start ??= DateTime.MinValue;
        end ??= DateTime.Today;

        var result = await _reportService.GenerateUsersReport(start.Value, end.Value);
        return Ok(new ApiOkResponse<List<UserReportDto>>(result));
    }

    [HttpGet("branches")]
    public async Task<ActionResult<List<BranchReportDto>>> GetBranchesReport(DateTime? start, DateTime? end)
    {
        start ??= DateTime.MinValue;
        end ??= DateTime.Today;

        var result = await _reportService.GenerateBranchesReport(start.Value, end.Value);
        return Ok(new ApiOkResponse<List<BranchReportDto>>(result));
    }

    [HttpGet("drivers")]
    public async Task<ActionResult<List<DriverReportDto>>> GetDriversReport(DateTime? start, DateTime? end)
    {
        start ??= DateTime.MinValue;
        end ??= DateTime.Today;

        var result = await _reportService.GenerateDriversReport(start.Value, end.Value);
        return Ok(new ApiOkResponse<List<DriverReportDto>>(result));
    }

    [HttpGet("plans")]
    public async Task<ActionResult<List<PlanReportDto>>> GetPlansReport(DateTime? start, DateTime? end)
    {
        start ??= DateTime.MinValue;
        end ??= DateTime.Today;

        var result = await _reportService.GeneratePlansReport(start.Value, end.Value);
        return Ok(new ApiOkResponse<List<PlanReportDto>>(result));
    }

    [HttpGet("orders")]
    public async Task<ActionResult<List<OrderReportDto>>> GetOrdersReport(DateTime? start, DateTime? end)
    {
        start ??= DateTime.MinValue;
        end ??= DateTime.Today;

        var result = await _reportService.GenerateOrdersReport(start.Value, end.Value);
        return Ok(new ApiOkResponse<List<OrderReportDto>>(result));
    }

    [HttpGet("pointOrders")]
    public async Task<ActionResult<List<OrderReportDto>>> GetPointOrdersReport(DateTime? start, DateTime? end)
    {
        start ??= DateTime.MinValue;
        end ??= DateTime.Today;

        var result = await _reportService.GeneratePointOrdersReport(start.Value, end.Value);
        return Ok(new ApiOkResponse<List<OrderReportDto>>(result));
    }
}