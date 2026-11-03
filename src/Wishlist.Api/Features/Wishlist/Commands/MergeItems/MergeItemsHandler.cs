using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Wishlist.Api.Domain;
using Wishlist.Api.Infrastructure;

namespace Wishlist.Api.Features.Wishlist.Commands.MergeItems;

public class MergeItemsHandler(IWishlistRepository repository) : IRequestHandler<MergeItemsCommand, CustomerWishlist>
{
    public async Task<CustomerWishlist> Handle(MergeItemsCommand request, CancellationToken ct)
    {
        var wishlist = await repository.GetWishlistAsync(request.CustomerId, ct)
            ?? new CustomerWishlist
            {
                CustomerId = request.CustomerId
            };

        foreach (var item in request.Items)
        {
            var alreadyExists = wishlist.Items.Any(i => i.ProductId == item.ProductId);
            if (!alreadyExists)
            {
                wishlist.Items.Add(new WishlistItem
                {
                    ProductId = item.ProductId, ProductName = item.ProductName, Price = item.Price, ImageUrl = item.ImageUrl, Section = item.Section, CategoryName = item.CategoryName,
                });
            }
        }

        return await repository.SaveWishlistAsync(wishlist, ct);
    }
}
