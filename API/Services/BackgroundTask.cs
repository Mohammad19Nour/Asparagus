using AsparagusN.Data;
using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Services;

public class BackgroundTask : IHostedService,IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private Timer _timer;

    public BackgroundTask(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(_addPlanDays,null,TimeSpan.Zero, TimeSpan.FromSeconds(1));
       return Task.CompletedTask;
    }

    private void _addPlanDays(object? state)
    {
        Task.Run(async () =>
        {
            try
            {
                if (DateTime.Now.DayOfWeek != DayOfWeek.Thursday)
                {
                    return;
                }

               /* var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var daysQuery = unitOfWork.Repository<AdminPlan>().GetQueryable();

                daysQuery =  daysQuery.Where(x => x.AvailableDate <= DateTime.Today.AddDays(-6));
                var res = await daysQuery.ToListAsync();
                foreach (var r in res)
                {
                    unitOfWork.Repository<AdminPlan>().Delete(r);
                }

                await unitOfWork.SaveChanges();
                Console.WriteLine("tt");*/
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Change(Timeout.Infinite,0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}