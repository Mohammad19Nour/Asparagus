using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.DTOs;
using AsparagusN.DTOs.OrderDtos;
using AsparagusN.Enums;

namespace AsparagusN.Interfaces;

public interface IOrderService
{
    Task<Order?> GetOrderByIdAsync(int orderId, string buyerEmail);
    Task<(bool Success,string Message)> AssignOrderToDriver(int orderId, int driverId,int priority);
    Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);

    Task<(Order? Order, string Message)> CreateOrderAsync(string buyerEmail, int basketId,
        NewOrderInfoDto newOrderInfoDto);

    public Task<(Order? Order, string Message)> CalcPriceOfOrder(string buyerEmail, int basketId,
        NewOrderInfoDto newOrderInfoDto);

    public Task<ICollection<Order>> GetOrderWithStatus(OrderStatus status);
}