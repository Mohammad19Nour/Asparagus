using AsparagusN.Data;
using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Enums;
using AsparagusN.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Services;

public class BackgroundTask : IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private bool _added = false;
    private Timer _timer;

    public BackgroundTask(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      //  _timer = new Timer(_addPlanDays, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        return Task.CompletedTask;
    }

    private void _addPlanDays(object? state)
    {
        Task.Run(async () =>
        {
            try
            {
                if (DateTime.Now.DayOfWeek != DayOfWeek.Thursday && _added)
                {
                    return;
                }

                var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var daysQuery = unitOfWork.Repository<AdminPlan>().GetQueryable();
                PlanType[] types = (PlanType[])Enum.GetValues(typeof(PlanType));


                if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
                {
                    var lastDay = await daysQuery.OrderByDescending(x => x.AvailableDate).FirstOrDefaultAsync();

                    // not added
                    if (lastDay == null || lastDay.AvailableDate != DateTime.Today.AddDays(8))
                    {
                        var query = daysQuery.Where(x
                            => x.AvailableDate <= DateTime.Today.AddDays(-6));

                        var res = await query.ToListAsync();
                        foreach (var r in res)
                        {
                            unitOfWork.Repository<AdminPlan>().Delete(r);
                        }

                        foreach (var planType in types)
                        {
                            if (planType == PlanType.CustomMealPlan) continue;
                            
                            for (var j = 2; j <= 8; j++)
                            {
                                var newDay = new AdminPlan
                                {
                                    AvailableDate = DateTime.Today.AddDays(j),
                                    PlanType = planType
                                };
                                unitOfWork.Repository<AdminPlan>().Add(newDay);
                            }
                        }

                        await unitOfWork.SaveChanges();
                        _added = true;
                        Console.WriteLine("tt");
                    }
                    else
                    {
                    }
                }
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
        _timer.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}