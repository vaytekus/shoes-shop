using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Wishlist.Api.Domain;
using Wishlist.Api.Infrastructure;

namespace Wishlist.Api.Features.Wishlist.Queries.GetWishlist;

public class GetWishlistHandler(IWishlistRepository repository) : IRequestHandler<GetWishlistQuery, CustomerWishlist>
{
    public async Task<CustomerWishlist> Handle(GetWishlistQuery request, CancellationToken ct)
    {
        return await repository.GetWishlistAsync(request.CustomerId, ct)
            ?? new CustomerWishlist { CustomerId = request.CustomerId };
    }
}
