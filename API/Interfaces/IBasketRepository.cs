using AsparagusN.Entities;

namespace AsparagusN.Interfaces;

public interface IBasketRepository
{
    Task<CustomerBasket?> GetBasketAsync(int id);
    Task<CustomerBasket?> UpdateBasket(CustomerBasket basket);
    Task<bool> DeleteBasket(int basketId);
}