using System.Text.Json;
using AsparagusN.Data.Entities;
using AsparagusN.Entities;
using AsparagusN.Interfaces;
using StackExchange.Redis;

namespace AsparagusN.Data;

public class BasketRepository : IBasketRepository
{
    private readonly IDatabase _database;
    public BasketRepository(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task<CustomerBasket?> GetBasketAsync(int id)
    {
        var data = await _database.StringGetAsync(id.ToString());
        return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data!);
    }
    public async Task<CustomerBasket?> UpdateBasket(CustomerBasket basket)
    {
        var created = await _database.StringSetAsync(basket.Id.ToString(),
            JsonSerializer.Serialize(basket),TimeSpan.FromDays(30));
        if (!created) return null;

        return await GetBasketAsync(basket.Id);
    }

    public async Task<bool> DeleteBasket(int basketId)
    {
        return await _database.KeyDeleteAsync(basketId.ToString());
    }
}