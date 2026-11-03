using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;
using Wishlist.Api.Domain;

namespace Wishlist.Api.Infrastructure;

public class WishlistRepository(IConnectionMultiplexer redis) : IWishlistRepository
{
    private static string Key(string customerId) => $"wishlist:{customerId}";
    private readonly IDatabase _db = redis.GetDatabase();

    public async Task<CustomerWishlist?> GetWishlistAsync(string customerId, CancellationToken ct = default)
    {
        var data = await _db.StringGetAsync(Key(customerId));
        return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerWishlist>((string)data!);
    }

    public async Task<CustomerWishlist> SaveWishlistAsync(CustomerWishlist wishlist, CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(wishlist);
        await _db.StringSetAsync(Key(wishlist.CustomerId), json);
        return wishlist;
    }

    public async Task DeleteWishlistAsync(string customerId, CancellationToken ct = default)
    {
        await _db.KeyDeleteAsync(Key(customerId));
    }
}
