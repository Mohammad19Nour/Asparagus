using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.DTOs;
using AsparagusN.Enums;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AsparagusN.Specifications.OrdersSpecifications;

namespace AsparagusN.Services;

public class StatisticService : IStatisticService
{
    private readonly IUnitOfWork _unitOfWork;

    public StatisticService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<StatisticDto> GetStatistics()
    {
        var result = new StatisticDto();
        await AddUserStatistics(result);
        result.TotalSales = await GetTotalSales();
        result.TotalSalesForThisMonth = await GetTotalSalesForThisMonth();
        result.TotalSalesForBranches = await GetTotalSalesForBranches();
        result.MonthlyTotalSalesForBranches = await GetMonthlyTotalSalesForBranches();
        return result;
    }

    private async Task AddUserStatistics(StatisticDto dto)
    {
        var userSpec = new CustomersSpecification(false);
        var allUsers = await _unitOfWork.Repository<AppUser>().ListWithSpecAsync(userSpec);

        var currentMonth = DateTime.Now.Date.Month;
        var currentYear = DateTime.Now.Date.Year;
        dto.NumberOfAllUsers = allUsers.Count();
        dto.NumberOfNewUsers = allUsers.Where(x =>
                currentYear == x.RegistrationDate.Date.Year && x.RegistrationDate.Date.Month == currentMonth).ToList()
            .Count;

        dto.NumberOfNewPlansForNewUsers = allUsers.Where(x =>
                x.IsMealPlanMember && x.RegistrationDate.Year == currentYear &&
                x.RegistrationDate.Month == currentMonth)
            .ToList().Count;
        dto.NumberOfUsersWithPlan = allUsers.Where(x => x.IsMealPlanMember).ToList().Count;
        dto.NumberOfFemales = allUsers.Where(x => x.Gender == Gender.Female).ToList().Count;
        dto.NumberOfMales = allUsers.Where(x => x.Gender == Gender.Male).ToList().Count;
        dto.NumberOf18_29Users = allUsers.Where(x => ageChecker(x.Birthday.NumberOfYears(), 1)).ToList().Count;
        dto.NumberOf30_100Users = allUsers.Where(x => ageChecker(x.Birthday.NumberOfYears(), 2)).ToList().Count;
    }

    private async Task<decimal> GetTotalSales()
    {
        var spec = new DoneOrdersWithoutPointsPaymentSpecification();
        var orders = await _unitOfWork.Repository<Order>().ListWithSpecAsync(spec);
        Console.WriteLine(orders.Count);
        return orders.Sum(x => x.GetTotal());
    }


    private async Task<decimal> GetTotalSalesForThisMonth()
    {
        var currentMonth = DateTime.Now.Date.Month;
        var currentYear = DateTime.Now.Date.Year;
        var spec = new DoneOrdersWithoutPointsPaymentSpecification(currentMonth, currentYear);
        var orders = await _unitOfWork.Repository<Order>().ListWithSpecAsync(spec);

        return orders.Sum(x => x.GetTotal());
    }

    private async Task<object> GetTotalSalesForBranches()
    {
        var startDate = DateTime.MinValue.Date;
        var endDate = DateTime.Now.Date;
        var spec = new DoneOrdersWithoutPointsPaymentSpecification(startDate, endDate);
        var orders = await _unitOfWork.Repository<Order>().ListWithSpecAsync(spec);
        var branches = await _unitOfWork.Repository<Branch>().ListAllAsync();

        var groupedOrders = orders.GroupBy(x => x.BranchId).Select(x => new
        {
            branchId = x.Key,
            branchName = branches.First(b => b.Id == x.Key).NameEN,
            totalBranchSales = x.Sum(y => y.GetTotal())
        }).ToList();

        var totalSales = orders.Sum(x => x.GetTotal());


        foreach (var bran in branches)
        {
            if (groupedOrders.Any(x => x.branchId == bran.Id)) continue;

            groupedOrders.Add(new
            {
                branchId = bran.Id,
                branchName = bran.NameEN,
                totalBranchSales = 0m
            });
        }

        groupedOrders = groupedOrders.OrderBy(x => x.branchId).ToList();

        return new
        {
            TotalSales = totalSales,
            branchesSales = groupedOrders
        };
    }

    private async Task<object> GetMonthlyTotalSalesForBranches()
    {
        var startDate = new DateTime(DateTime.Now.Date.Year);
        Console.WriteLine(startDate);

        var endDate = DateTime.Now.Date;
        var spec = new DoneOrdersWithoutPointsPaymentSpecification(startDate, endDate);
        var orders = await _unitOfWork.Repository<Order>().ListWithSpecAsync(spec);

        var groupedOrders = orders.GroupBy(x => new { x.BranchId, x.OrderDate.Date.Month }).Select(x => new
        {
            branchId = x.Key.BranchId,
            month = x.Key.Month,
            totalBranchSales = x.Sum(y => y.GetTotal())
        }).ToList();

        var branches = await _unitOfWork.Repository<Branch>().ListAllAsync();
        var resultList = new List<object>();

        foreach (var bran in branches)
        {
            var item = groupedOrders.Where(x => x.branchId == bran.Id)
                .GroupBy(x => x.month).Select(x =>
                    new
                    {
                        month = x.Key,
                        monthlySales = x.Sum(y => y.totalBranchSales)
                    }).ToList();

            var branch = new { branchName = bran.NameEN, monthlySales = new List<decimal>(12) };

            for (int j = 1; j <= 12; j++)
            {
                var sales = 0m;
                var month = item.FirstOrDefault(x => x.month == j);

                if (month != null)
                {
                    sales = month.monthlySales;
                }

                branch.monthlySales.Add(sales);
            }

            resultList.Add(branch);
        }

        return resultList;
    }

    private bool ageChecker(int numberOfYears, int age)
    {
        if (age == 1) return numberOfYears is >= 18 and <= 29;
        return numberOfYears is >= 30 and <= 100;
    }
}