﻿using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.DTOs;
using AsparagusN.DTOs.DriverDtos;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AsparagusN.Specifications.OrdersSpecifications;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers;
[Authorize]
public class DriversController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;
    private readonly IMapper _mapper;

    public DriversController(IUnitOfWork unitOfWork, INotificationService notificationService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
        _mapper = mapper;
    }

    [HttpGet("orders")]
    public async Task<ActionResult> PlanOrders()
    {
        var email = User.GetEmail();
        var driver = await _unitOfWork.Repository<Driver>().GetQueryable()
            .Where(c => c.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();

        if (driver == null) return Ok(new ApiResponse(404, "driver not found"));

        var users = await _unitOfWork.Repository<AppUser>().ListAllAsync();

        var spec = new PlanDayOrdersForDriverWithStatusSpecification(driver.Id, PlanOrderStatus.Ready);
        var orders = await _unitOfWork.Repository<UserPlanDay>().ListWithSpecAsync(spec);

        var orderToReturn = _mapper.Map<List<OrderUserPlanDayDto>>(orders);
        for (int j = 0; j < orders.Count; j++)
        {
            var user = users.First(c => c.Id == orders[j].UserPlan.AppUserId);
            orderToReturn[j].PhoneNumber = user.PhoneNumber;
            orderToReturn[j].Username = user.FullName;
            orderToReturn[j].PictureUrl = user.PictureUrl;
        }

        return Ok(new ApiOkResponse<List<OrderUserPlanDayDto>>(orderToReturn));
    }

    [HttpPut]
    public async Task<ActionResult> ChangeDriverStatus()
    {
        var email = User.GetEmail();
        var driver = await _unitOfWork.Repository<Driver>().GetQueryable()
            .Where(c => c.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();

        if (driver == null) return Ok(new ApiResponse(404, "driver not found"));

        driver.Status = DriverStatus.Delivering;
        _unitOfWork.Repository<Driver>().Update(driver);

        if (await _unitOfWork.SaveChanges())
            return (Ok(new ApiResponse(200, "Updated")));
        return Ok(new ApiResponse(400, "Failed to update status of driver"));
    }

    [HttpPut("delivered")]
    public async Task<ActionResult> UpdateOrderStatus([FromQuery] int orderId)
    {
        var email = User.GetEmail();
        var driver = await _unitOfWork.Repository<Driver>().GetQueryable()
            .Where(c => c.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();

        if (driver == null) return Ok(new ApiResponse(404, "driver not found"));

        var orderSpec = new PlanDayOrderWithDriverSpecification(orderId);
        var order = await _unitOfWork.Repository<UserPlanDay>().GetEntityWithSpec(orderSpec);

        if (order == null) return Ok(new ApiResponse(404, "Order not found"));
        order.DayOrderStatus = PlanOrderStatus.Delivered;
        _unitOfWork.Repository<UserPlanDay>().Update(order);

        var notDeliveredOrdersSpec =
            new PlanDayOrdersForDriverWithStatusSpecification(driver.Id, PlanOrderStatus.Ready);
        var notDelivered = await _unitOfWork.Repository<UserPlanDay>().ListWithSpecAsync(notDeliveredOrdersSpec);

        if (notDelivered.Count == 0)
        {
            driver.Status = DriverStatus.Idle;
            _unitOfWork.Repository<Driver>().Update(driver);
        }

        if (!await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(400, "Failed to update status of order"));

        if (notDelivered.Count > 0)
        {
            var customersSpec = new CustomersSpecification(false);
            var users = await _unitOfWork.Repository<AppUser>().ListWithSpecAsync(customersSpec);

            notDelivered = notDelivered.OrderBy(c => c.Priority).ToList();
            for (int j = 0; j < Math.Min(notDelivered.Count, 2); j++)
            {
                var nxrOrder = notDelivered[j];

                var userEmail = users.First(c => c.Id == nxrOrder.UserPlan.AppUserId).Email.ToLower();

                await _notificationService.NotifyUserByEmail(userEmail, "طلبك في طريقه اليك", "Your order on the way");
            }
        }

        return (Ok(new ApiResponse(200, "Updated")));
    }

    [HttpGet("info")]
    public async Task<ActionResult<DriverDto>> GetInfo()
    {
        var email = User.GetEmail();
        var spec = new DriverSpecification(email.ToLower());
        var driver = await _unitOfWork.Repository<Driver>().GetEntityWithSpec(spec);

        if (driver == null) return Ok(new ApiResponse(404, "driver not found"));

        return Ok(new ApiOkResponse<DriverDto>(_mapper.Map<DriverDto>(driver)));
    }
}