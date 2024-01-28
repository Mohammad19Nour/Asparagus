using AsparagusN.Entities.OrderAggregate;
using AsparagusN.Interfaces;

namespace AsparagusN.Services;

public class OrderService : IOrderService
{

    public OrderService(IUnitOfWork unitOfWork,IBasketRepository basketRepository)
    {
    }

    public Task<Order> GetOrderByIdAsync(int orderId, string buyerEmail)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        throw new NotImplementedException();
    }

    public Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethod, string basketId, DeliveryAddress shippingAddress)
    {
        throw new NotImplementedException();
    }
}