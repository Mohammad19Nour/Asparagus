using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.Enums;
using AsparagusN.Interfaces;
using AsparagusN.Specifications.OrdersSpecifications;
using AutoMapper;

namespace AsparagusN.Services;

public class UserPlanOrderService : IUserPlanOrderService
{
    private readonly ILocationService _locationService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;

    public UserPlanOrderService(ILocationService locationService, IMapper mapper, IUnitOfWork unitOfWork,
        INotificationService notificationService)
    {
        _locationService = locationService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
    }
    public async Task<(bool Success, string Message)> AssignPlanDayOrderToDriver(int orderId, int driverId, int priority)
    {
        var order = await _unitOfWork.Repository<UserPlanDay>().GetByIdAsync(orderId);

        if (order == null) return (false, "Order not found");

        var driver = await _unitOfWork.Repository<Driver>().GetByIdAsync(driverId);
        if (driver == null) return (false, "Driver not found");
        
        if (order.DayOrderStatus != PlanOrderStatus.Ready) return (false, "Order isn't pending");

        if (driver.Status != DriverStatus.Idle)
            return (false, $" Can't assign to this driver because he is {driver.Status}");

        var allSpec = new PlanDayOrdersForDriverWithStatusSpecification(driverId, PlanOrderStatus.Ready);
        var driverOrders = await _unitOfWork.Repository<UserPlanDay>().ListWithSpecAsync(allSpec);

        var priorities = driverOrders.Select(x => x.Priority).ToList();
        priorities.Sort();
        if (priorities.Contains(priority))
        {
            var available = 1;
         
            foreach (var p in priorities.TakeWhile(p => p == available))
            {
                available++;
            }

            return (false, $"Priority is already chosen,you can choose {available} priority");
        }

        order.Driver = driver;
        order.Priority = priority;
        _unitOfWork.Repository<UserPlanDay>().Update(order);

        if (await _unitOfWork.SaveChanges())
            return (true, "Assigned successfully");

        return (false, "Failed to assign driver");
    }
}