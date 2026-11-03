using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Wishlist.Api.Domain;

namespace Wishlist.Api.Infrastructure;

public interface IWishlistRepository
{
    Task<CustomerWishlist?> GetWishlistAsync(string customerId, CancellationToken ct = default);
    Task<CustomerWishlist> SaveWishlistAsync(CustomerWishlist wishlist, CancellationToken ct = default);
    Task DeleteWishlistAsync(string customerId, CancellationToken ct = default);
}
