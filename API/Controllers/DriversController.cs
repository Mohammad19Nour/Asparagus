using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AsparagusN.Specifications.OrdersSpecifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers;

public class DriversController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;

    public DriversController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("orders")]
    public async Task<ActionResult> PlanOrders()
    {
        var email = User.GetEmail();
        var driver = await _unitOfWork.Repository<Driver>().GetQueryable()
            .Where(c => c.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();

        if (driver == null) return Ok(new ApiResponse(404, "driver not found"));

        var spec = new PlanDayOrdersForDriverWithStatusSpecification(driver.Id,PlanOrderStatus.Ready);
        var orders = await _unitOfWork.Repository<UserPlanDay>().ListWithSpecAsync(spec);
        return Ok(orders);
    }
    [HttpPut("{driverId:int}")]
    public async Task<ActionResult> ChangeDriverStatus(int driverId)
    {
        var driver = await _unitOfWork.Repository<Driver>().GetByIdAsync(driverId);

        if (driver == null) return Ok(new ApiResponse(404, "Driver not found"));

        driver.Status = DriverStatus.Delivering;
        _unitOfWork.Repository<Driver>().Update(driver);

        if (await _unitOfWork.SaveChanges())
            return (Ok(new ApiResponse(200, "Updated")));
        return Ok(new ApiResponse(400, "Failed to update status of driver"));
    }

    [HttpPut("delivered")]
    public async Task<ActionResult> UpdateOrderStatus([FromQuery] int orderId)
    {
        var orderSpec = new PlanDayOrderWithDriverSpecification(orderId);
        var order = await _unitOfWork.Repository<UserPlanDay>().GetEntityWithSpec(orderSpec);

        if (order == null) return Ok(new ApiResponse(404, "Order not found"));

        order.DayOrderStatus = PlanOrderStatus.Delivered;
        _unitOfWork.Repository<UserPlanDay>().Update(order);

        if (await _unitOfWork.SaveChanges())
            return (Ok(new ApiResponse(200, "Updated")));
        return Ok(new ApiResponse(400, "Failed to update status of order"));
    }
}