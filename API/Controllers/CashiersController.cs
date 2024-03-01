using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.Data.Entities.Meal;
using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.DTOs.OrderDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AsparagusN.Specifications.OrdersSpecifications;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers;

public class CashiersController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOrderService _orderService;

    public CashiersController(IUnitOfWork unitOfWork, IMapper mapper, IOrderService orderService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _orderService = orderService;
    }

    [HttpGet("orders")]
    public async Task<ActionResult<List<OrderDto>>> GetOrders(OrderStatus status)
    {
        var email = User.GetEmail();
        var cashier = await _unitOfWork.Repository<Cashier>().GetQueryable()
            .Where(c => c.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();

        if (cashier == null) return Ok(new ApiResponse(404, "Cashier not found"));

        var orderSpec = new OrdersInBranchSpecification(cashier.BranchId, status);
        var orders = await _unitOfWork.Repository<Order>().ListWithSpecAsync(orderSpec);
        orders = orders.OrderByDescending(c => c.OrderDate).ToList();

        return Ok(new ApiOkResponse<List<OrderDto>>(_mapper.Map<List<OrderDto>>(orders)));
    }

    [HttpPut("orders/{orderId:int}")]
    public async Task<ActionResult> ChangeOrder(int orderId)
    {
        var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);

        if (order == null) return Ok(new ApiResponse(400, "Order not found"));
        if (order.Status == OrderStatus.Done) return Ok(new ApiResponse(400, "Order already done"));

        order.Status = OrderStatus.Done;
        _unitOfWork.Repository<Order>().Update(order);

        if (await _unitOfWork.SaveChanges()) return Ok(new ApiResponse(200, "Order become in done status"));
        return Ok(new ApiResponse(400, "Failed to update order status"));
    }

    [HttpPut("points")]
    public async Task<ActionResult> ChangeOrder(int mealId, string userEmail)
    {
        var email = User.GetEmail();
        var result = await _orderService.CreateCashierOrderAsync(userEmail, mealId, email);


        if (result.Order != null) return Ok(new ApiResponse(200, "Order created"));
        return Ok(new ApiResponse(400, "Failed to create order"));
    }
}