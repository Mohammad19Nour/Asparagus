using AsparagusN.Entities;

namespace AsparagusN.Interfaces;

public interface IBasketRepository
{
    Task<CustomerBasket> GetBasketAsync(string id);
    Task<CustomerBasket> UpdateBasket(CustomerBasket basket);
    Task<bool> DeleteBasket(string basketId);
}