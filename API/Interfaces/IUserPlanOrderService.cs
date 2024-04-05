namespace AsparagusN.Interfaces;

public interface IUserPlanOrderService
{
    Task<(bool Success,string Message)> AssignPlanDayOrderToDriver(int orderId, int driverId,int priority);

}