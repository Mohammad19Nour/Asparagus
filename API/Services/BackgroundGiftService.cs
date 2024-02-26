using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;

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

        _timer = new Timer(_doWork, null, delay, TimeSpan.FromDays(1));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private async void _doWork(object? state)
    {
        int currentMonth = DateTime.Now.Month;
        int currentDay = DateTime.Now.Day;

        var scope = _scopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        bool ok = true;
        while (ok)
        {
            var customerSpec = new CustomersSpecification(false);

            var users = await unitOfWork.Repository<AppUser>().ListWithSpecAsync(customerSpec);
            users = users.Where(c =>
                c.Birthday.Month == currentMonth && c.Birthday.Day == currentDay && c.EmailConfirmed).ToList();

            var gifts = await unitOfWork.Repository<UserGift>().ListAllAsync();
            foreach (var userGift in gifts)
            {
                unitOfWork.Repository<UserGift>().Delete(userGift);
            }

            foreach (var user in users)
            {
                unitOfWork.Repository<UserGift>().Add(new UserGift
                {
                    User = user,
                });
            }

            if (await unitOfWork.SaveChanges()) ok = false;
        }
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}