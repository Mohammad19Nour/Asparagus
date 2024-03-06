using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.DTOs.AddressDtos;
using AsparagusN.DTOs.OrderDtos;
using AsparagusN.Enums;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Order = AsparagusN.Data.Entities.OrderAggregate.Order;

namespace AsparagusN.Services;

public class BackgroundGiftService : IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private Timer _timer;

    public BackgroundGiftService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.Now;
        var tomorrow = now.AddDays(1).Date;
        var delay = tomorrow - now;

        _timer = new Timer(_doWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private async void _doWork(object? state)
    {
        var currentMonth = DateTime.Now.Month;
        var currentDay = DateTime.Now.Day;
        var currentYear = DateTime.Now.Year;

        var scope = _scopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        var customerSpec = new CustomersSpecification();

        var users = await unitOfWork.Repository<AppUser>().ListWithSpecAsync(customerSpec);
        users = users.Where(c =>
            c.Birthday.Month == currentMonth && c.Birthday.Day == currentDay && c.EmailConfirmed).ToList();

        var gifts = await unitOfWork.Repository<GiftSelection>().ListAllAsync();
        var mealId = gifts.Where(c => c.Month == currentMonth).Select(x => x.MealId).FirstOrDefault();
        var usersWhoSentToThemGift = await unitOfWork.Repository<Order>().GetQueryable()
            .Where(c => c.PaymentType == PaymentType.Gift && c.OrderDate.Month == currentMonth &&
                        c.OrderDate.Date.Day == currentDay && c.OrderDate.Year == currentYear).Select(c => c.BuyerId)
            .ToListAsync();

        users = users.Where(c => !usersWhoSentToThemGift.Contains(c.Id)).ToList();
        int cnt = 50;
        bool ok = true;

        while (ok && (cnt-- > 0) && mealId != null)
        {
            using (var transaction = unitOfWork.BeginTransaction())
            {
                bool done = true;

                foreach (var user in users)
                {
                    var res = await orderService.CreateGiftOrderAsync(user, mealId.Value);
                    done &= res.Success;
                }

                if (done)
                {
                    foreach (var user in users)
                        await notificationService.NotifyUserByEmail(user.Email, "TestGift", "TestGift");

                    ok = false;
                    await transaction.CommitAsync();
                }
                else
                {
                    ok = true;
                    await transaction.RollbackAsync();
                }
            }
        }
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}