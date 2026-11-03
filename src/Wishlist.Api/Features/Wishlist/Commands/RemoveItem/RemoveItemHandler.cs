using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Wishlist.Api.Infrastructure;

namespace Wishlist.Api.Features.Wishlist.Commands.RemoveItem;

public class RemoveItemHandler(IWishlistRepository repository) : IRequestHandler<RemoveItemCommand>
{
    public async Task Handle(RemoveItemCommand request, CancellationToken ct)
    {
        var wishlist = await repository.GetWishlistAsync(request.CustomerId, ct);
        if (wishlist is null)
        {
            return;
        }

        wishlist.Items.RemoveAll(i => i.ProductId == request.ProductId);
        await repository.SaveWishlistAsync(wishlist, ct);
    }
}
