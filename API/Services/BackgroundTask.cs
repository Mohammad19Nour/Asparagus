using AsparagusN.Data;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
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
       // _addPlanDays(null);

        var now = DateTime.Now;
        var daysUntilNextThursday = ((int)DayOfWeek.Thursday - (int)now.DayOfWeek + 7) % 7;

        var nextThursday = now.AddDays(daysUntilNextThursday).Date;

        var timeUntilNextThursday = nextThursday - now;
        if (timeUntilNextThursday < TimeSpan.Zero) timeUntilNextThursday = TimeSpan.Zero;
        
        _timer = new Timer(_addPlanDays, null, timeUntilNextThursday, TimeSpan.FromDays(7));
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
                var daysQuery = unitOfWork.Repository<AdminPlanDay>().GetQueryable();
                PlanTypeEnum[] types = (PlanTypeEnum[])Enum.GetValues(typeof(PlanTypeEnum));


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
                            unitOfWork.Repository<AdminPlanDay>().Delete(r);
                        }

                        foreach (var planType in types)
                        {
                            if (planType == PlanTypeEnum.CustomMealPlan) continue;

                            for (var j = 2; j <= 8; j++)
                            {
                                var newDay = new AdminPlanDay
                                {
                                    AvailableDate = DateTime.Today.AddDays(j),
                                    PlanType = planType
                                };
                                unitOfWork.Repository<AdminPlanDay>().Add(newDay);
                            }
                        }

                        await unitOfWork.SaveChanges();
                        _added = true;
                    }
                    else
                    {
                    }
                }
                else // => _added = false
                {
                    var startDay = DateTime.Today;
                    while (startDay.DayOfWeek != DayOfWeek.Saturday)
                    {
                        startDay = startDay.AddDays(-1);
                    }

                    foreach (var planType in types)
                    {
                        if (planType == PlanTypeEnum.CustomMealPlan) continue;

                        for (var j = 0; j <= 6; j++)
                        {
                            var newDay = new AdminPlanDay
                            {
                                AvailableDate = startDay.AddDays(j),
                                PlanType = planType
                            };
                            unitOfWork.Repository<AdminPlanDay>().Add(newDay);
                        }
                    }

                    await unitOfWork.SaveChanges();
                    _added = true;
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