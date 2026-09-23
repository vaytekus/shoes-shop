using System.Text.Json;
using Basket.Api.Domain;
using StackExchange.Redis;

namespace Basket.Api.Infrastructure;

public class BasketRepository(IConnectionMultiplexer redis) : IBasketRepository
{
    private  readonly IDatabase _db = redis.GetDatabase();

    public async Task<CustomerBasket?> GetBasketAsync(string customerId, CancellationToken ct = default)
    {
        var data = await _db.StringGetAsync(customerId);
        return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>((string)data!);
    }

    public async Task<CustomerBasket> SaveBasketAsync(CustomerBasket basket, CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(basket);
        await _db.StringSetAsync(basket.CustomerId, json);
        return basket;
    }

    public async Task DeleteBasketAsync(string customerId, CancellationToken ct = default)
    {
        await _db.KeyDeleteAsync(customerId);
    }
}
