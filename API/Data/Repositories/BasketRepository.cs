using System.Text.Json;
using AsparagusN.Data.Entities;
using AsparagusN.Entities;
using AsparagusN.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace AsparagusN.Data;

public class BasketRepository : IBasketRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IDatabase _database;

    public BasketRepository(IConnectionMultiplexer redis, DataContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _database = redis.GetDatabase();
    }

    public async Task<CustomerBasket?> GetBasketAsync(int id)
    {
        return await _context.CustomerBaskets.Where(x => x.Id == id).FirstOrDefaultAsync();
        // var data = await _database.StringGetAsync(id.ToString());
        //return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data!);
    }

    public async Task<CustomerBasket?> UpdateBasket(CustomerBasket basket)
    {
        var bas = await _context.CustomerBaskets.Where(x => x.Id == basket.Id)
            .Include(t=>t.Items)
            .FirstOrDefaultAsync();
      
        Console.WriteLine(basket.Id);

        if (bas == null)
        {
             _context.CustomerBaskets.Add(basket);
             bas = basket;
        }
        else
        {
            bas.Items.Clear();
            bas.Items = basket.Items;
            bas.TotalPrice = basket.TotalPrice;
            
            _context.CustomerBaskets.Attach(bas);
            _context.Entry(bas).State = EntityState.Modified;
        }
        await _context.SaveChangesAsync();
        return bas;
        // var created = await _database.StringSetAsync(basket.Id.ToString(),
        //   JsonSerializer.Serialize(basket),TimeSpan.FromDays(30));
        //if (!created) return null;
        //return await GetBasketAsync(basket.Id);
    }

    public async Task<bool> DeleteBasket(int basketId)
    {
        var b = await _context.CustomerBaskets.Where(x => x.Id == basketId).FirstOrDefaultAsync();
        _context.CustomerBaskets.Remove(b);
        return await _context.SaveChangesAsync() > 0;
        //return await _database.KeyDeleteAsync(basketId.ToString());
    }
}