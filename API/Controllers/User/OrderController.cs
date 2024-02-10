﻿using AsparagusN.DTOs;
using AsparagusN.DTOs.OrderDtos;
using AsparagusN.Entities;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.User;

public class OrderController :BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrderController(IUnitOfWork unitOfWork,IOrderService orderService,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _orderService = orderService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult> CreateOrder(NewOrderDto newOrderDto)
    {
        var email = HttpContext.User.GetEmail();
        if (email == null) return Ok(new ApiResponse(404, "User not found"));

        var address = _mapper.Map<AddressDto,Address>(newOrderDto.ShipToAddress);

        var order = await _orderService.CreateOrderAsync(email, newOrderDto.BasketId, address);

        if (order == null) return Ok(new ApiResponse(400, "Failed to add order"));

        return Ok(new ApiOkResponse<OrderDto>(_mapper.Map<OrderDto>(order)));
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser()
    {
        var email = HttpContext.User.GetEmail();
        if (email == null) return Ok(new ApiResponse(404, "User not found"));
        
        var orders = await _orderService.GetOrdersForUserAsync(email);
        return Ok(new ApiOkResponse<IReadOnlyList<OrderDto>>(_mapper.Map<IReadOnlyList<OrderDto>>(orders)));
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDto>> GetOrderByIdForUser(int id)
    {
        var email = HttpContext.User.GetEmail();
        if (email == null) return Ok(new ApiResponse(404, "User not found"));
        
        var order = await _orderService.GetOrderByIdAsync(id,email);
        
        if (order == null) return Ok(new ApiResponse(404, "Order not found"));
        
        return Ok(new ApiOkResponse<OrderDto>(_mapper.Map<OrderDto>(order)));
    }
}