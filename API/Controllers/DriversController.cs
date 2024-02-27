using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AsparagusN.Specifications.OrdersSpecifications;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers;

public class DriversController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;

    public DriversController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
    public async Task<ActionResult> UpdateOrderStatus([FromQuery] int orderId, [FromQuery] int driverId)
    {
        var orderSpec = new OrderWithDriverSpecification(orderId, driverId);
        var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(orderSpec);

        if (order == null) return Ok(new ApiResponse(404, "Order not found"));

        order.Status = OrderStatus.Done;
        _unitOfWork.Repository<Order>().Update(order);

        if (await _unitOfWork.SaveChanges())
            return (Ok(new ApiResponse(200, "Updated")));
        return Ok(new ApiResponse(400, "Failed to update status of order"));
    }
}