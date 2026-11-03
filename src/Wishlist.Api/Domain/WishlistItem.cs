using System;

namespace Wishlist.Api.Domain;

public class WishlistItem
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string Section { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
}
