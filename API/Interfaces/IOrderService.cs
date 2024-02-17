using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.DTOs;

namespace AsparagusN.Interfaces;

public interface IOrderService
{
    Task<Order?> GetOrderByIdAsync(int orderId, string buyerEmail);
    Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);

    Task<Order?> CreateOrderAsync(string buyerEmail, int basketId,
        Address shippingAddress);
}