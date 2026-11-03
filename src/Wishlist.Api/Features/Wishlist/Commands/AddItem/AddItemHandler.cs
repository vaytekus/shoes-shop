using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Wishlist.Api.Domain;
using Wishlist.Api.Infrastructure;

namespace Wishlist.Api.Features.Wishlist.Commands.AddItem;

public class AddItemHandler(IWishlistRepository repository) : IRequestHandler<AddItemCommand, CustomerWishlist>
{
    public async Task<CustomerWishlist> Handle(AddItemCommand request, CancellationToken ct)
    {
        var wishlist = await repository.GetWishlistAsync(request.CustomerId, ct)
            ?? new CustomerWishlist
            {
                CustomerId = request.CustomerId
            };

        var alreadyExists = wishlist.Items.Any(i => i.ProductId == request.ProductId);

        if (!alreadyExists)
        {
            wishlist.Items.Add(new WishlistItem
            {
                ProductId = request.ProductId, ProductName = request.ProductName, Price = request.Price, ImageUrl = request.ImageUrl, Section = request.Section, CategoryName = request.CategoryName,
            });
        }

        return await repository.SaveWishlistAsync(wishlist, ct);
    }
}
