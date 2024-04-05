using System.Reflection.Metadata;
using AsparagusN.Data;
using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.DTOs.OrderDtos;
using AsparagusN.DTOs.ReportDtos;
using AsparagusN.Enums;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AsparagusN.Specifications.OrdersSpecifications;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Services;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<List<BranchReportDto>> GenerateBranchesReport(DateTime startDate, DateTime endDate)
    {
        var orderSpec = new DoneOrdersSpecification(startDate, endDate);
        var orders = await _unitOfWork.Repository<Order>().ListWithSpecAsync(orderSpec);
        var branches = await _unitOfWork.Repository<Branch>().ListAllAsync();

        var bList = new List<BranchReportDto>();
        foreach (var b in branches)
        {
            var add = new BranchReportDto();
            add.BranchName = b.NameEN;
            add.NumberOfOrders = orders.Count(x => x.BranchId == b.Id);
            add.TotalSales = orders.Where(x => x.BranchId == b.Id).Sum(y => y.GetTotal());
            bList.Add(add);
        }

        return bList;
    }

    public async Task<List<UserReportDto>> GenerateUsersReport(DateTime startDate, DateTime endDate)
    {
        var customerSpec = new CustomersSpecification(false);
        var users = await _unitOfWork.Repository<AppUser>().ListWithSpecAsync(customerSpec);
        users = users.Where(c => c.RegistrationDate.Date >= startDate && c.RegistrationDate.Date <= endDate).ToList();
        var uList = _mapper.Map<List<UserReportDto>>(users);

        var orderSpec = new DoneOrdersSpecification(startDate, DateTime.Now);
        var orders = await _unitOfWork.Repository<Order>().ListWithSpecAsync(orderSpec);


        var plans = await _unitOfWork.Repository<UserPlan>().ListAllAsync();


        foreach (var user in uList)
        {
            var email = user.Email.ToLower();
            user.NumberOfOrders = orders.Count(x => x.BuyerEmail.ToLower() == email.ToLower());
            user.NumberOfPlans = plans.Count(c => c.AppUserId == user.Id);
        }

        return uList;
    }

    public async Task<SalesDto> GenerateSalesReport(DateTime startDate, DateTime endDate)
    {
        var reportDto = new SalesDto();
        var orderSpec = new DoneOrdersWithoutPointsPaymentSpecification(startDate, endDate);
        var orders = await _unitOfWork.Repository<Order>().ListWithSpecAsync(orderSpec);

        reportDto.NumberOfOrders = orders.Count;
        reportDto.TotalOrderSales = orders.Sum(y => y.GetTotal());

        var plans = await _unitOfWork.Repository<UserPlan>().GetQueryable()
            .Where(c => c.CreatedDate.Date >= startDate && c.CreatedDate.Date <= endDate).ToListAsync();
        reportDto.TotalPlanSales = plans.Sum(c => c.Price);
        reportDto.NumberOfPlans = plans.Count;

        return reportDto;
    }

    public async Task<List<PlanReportDto>> GeneratePlansReport(DateTime startDate, DateTime endDate)
    {
        var plans = await _unitOfWork.Repository<UserPlan>().GetQueryable()
            .Where(c => c.CreatedDate.Date >= startDate && c.CreatedDate.Date <= endDate).ToListAsync();

        var users = await _unitOfWork.Repository<AppUser>().ListAllAsync();

        var result = new List<PlanReportDto>();
        foreach (var plan in plans)
        {
            var tmp = _mapper.Map<PlanReportDto>(plan);
            var user = users.FirstOrDefault(x => x.Id == plan.AppUserId);
            tmp.Username = user == null ? "Not found" : user.FullName;
            result.Add(tmp);
        }

        return result;
    }

    public async Task<List<DriverReportDto>> GenerateDriversReport(DateTime startDate, DateTime endDate)
    {
        var spec = new DriverSpecification();
        var drivers = await _unitOfWork.Repository<Driver>().ListWithSpecAsync(spec);

        var resultDrivers = _mapper.Map<List<DriverReportDto>>(drivers);
        return resultDrivers;
    }

    public async Task<List<OrderReportDto>> GenerateOrdersReport(DateTime startDate, DateTime endDate)
    {
        var orderSpec = new DoneOrdersWithoutPointsPaymentSpecification(startDate, endDate);
        var orders = await _unitOfWork.Repository<Order>().ListWithSpecAsync(orderSpec);

        var result = _mapper.Map<List<OrderReportDto>>(orders);
        return result;
    }

    public async Task<List<OrderReportDto>> GeneratePointOrdersReport(DateTime startDate, DateTime endDate)
    {
        var orderSpec = new DoneOrderWithPointsPaymentOnlySpecification(startDate, endDate);
        var orders = await _unitOfWork.Repository<Order>().ListWithSpecAsync(orderSpec);

        var result = _mapper.Map<List<OrderReportDto>>(orders.ToList());
        return result;
    }

    public async Task<List<BookingReportDto>> GenerateBookingsReport(DateTime startDate, DateTime endDate)
    {
        var query = _unitOfWork.Repository<Booking>().GetQueryable();
        query = query.Include(c => c.Car);
        query = query.Include(c => c.User);
        query = query.Where(c => c.StartTime >= startDate && c.EndTime <= endDate);
        query = query.OrderBy(c => c.StartTime);

        var bookings = await query.ToListAsync();

        var resultList = new List<BookingReportDto>();

        foreach (var booking in bookings)
        {
            var tmp = _mapper.Map<BookingReportDto>(booking.User);
            tmp.StartTime = booking.StartTime;
            tmp.EndTime = booking.EndTime;
            tmp.City = booking.Car.City;
            resultList.Add(tmp);
        }

        return resultList;
    }
}