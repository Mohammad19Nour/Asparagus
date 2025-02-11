﻿using AsparagusN.Data.Entities.Identity;
using AsparagusN.DTOs.OrderDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers;

public class OrdersController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public OrdersController(IUnitOfWork unitOfWork, IOrderService orderService, IMapper mapper,
        UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _orderService = orderService;
        _mapper = mapper;
        _userManager = userManager;
    }

    [Authorize(Roles = nameof(Roles.User))]
    [HttpPost]
    public async Task<ActionResult> CreateOrder(NewOrderInfoDto newOrderInfo)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "User not found"));

        if (newOrderInfo.PaymentType == PaymentType.Gift)
            return Ok(new ApiResponse(400, "Wrong payment type"));

        var result = await _orderService.CreateOrderAsync(user.Email, user.Id, newOrderInfo);

        if (result.Order == null) return Ok(new ApiResponse(400, result.Message));

        return Ok(new ApiOkResponse<OrderDto>(_mapper.Map<OrderDto>(result.Order)));
    }

    [Authorize(Roles = nameof(DashboardRoles.Order) + "," + nameof(Roles.Admin))]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrders(OrderStatus status)
    {
        var orders = await _orderService.GetOrderWithStatus(status);
        return Ok(new ApiOkResponse<IReadOnlyList<OrderDto>>(_mapper.Map<IReadOnlyList<OrderDto>>(orders)));
    }
/*
    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDto>> GetOrderByIdForUser(int id)
    {
        var email = HttpContext.User.GetEmail();
        if (email == null) return Ok(new ApiResponse(404, "User not found"));

        var order = await _orderService.GetOrderByIdAsync(id, email);

        if (order == null) return Ok(new ApiResponse(404, "Order not found"));

        return Ok(new ApiOkResponse<OrderDto>(_mapper.Map<OrderDto>(order)));
    }
*/

    private async Task<AppUser?> _getUser()
    {
        var email = HttpContext.User.GetEmail();
        return await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}