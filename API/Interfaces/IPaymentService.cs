using AsparagusN.Data.Entities;

namespace AsparagusN.Interfaces;

public interface IPaymentService
{
    Task<CustomerBasket?> CreatePaymentIntent(int basketId);
}