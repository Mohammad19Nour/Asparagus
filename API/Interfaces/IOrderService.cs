using AsparagusN.Entities.OrderAggregate;

namespace AsparagusN.Interfaces;

public interface IOrderService
{
    Task<Order> GetOrderByIdAsync(int orderId, string buyerEmail);
    Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);

    Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethod, string basketId,
        DeliveryAddress shippingAddress);

}