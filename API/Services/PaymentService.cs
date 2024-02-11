using AsparagusN.Data.Entities;
using AsparagusN.Interfaces;
using StackExchange.Redis;
using Stripe;
using Order = AsparagusN.Entities.OrderAggregate.Order;

namespace AsparagusN.Services;

public class PaymentService : IPaymentService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public PaymentService(IBasketRepository basketRepository,IUnitOfWork unitOfWork,IConfiguration configuration)
    {
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<CustomerBasket?> CreatePaymentIntent(int orderId)
    {
        StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
        var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);

        if (order == null) return null;
        
        var shippingPrice = 0m; //m for money
        var service = new PaymentIntentService();
        PaymentIntent intent;
        var options = new PaymentIntentCreateOptions
        {
            Amount =(long) order.Subtotal * 100,
            Currency = "aed",
            PaymentMethodTypes = new List<string>{"cards"}
        };
        intent = await service.CreateAsync(options);
        return null;
    }
}