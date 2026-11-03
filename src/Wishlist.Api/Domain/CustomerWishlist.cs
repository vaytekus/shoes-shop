using System.Collections.Generic;

namespace Wishlist.Api.Domain;

public class CustomerWishlist
{
    public string CustomerId { get; set; } = null!;
    public List<WishlistItem> Items { get; set; } = [];
}
