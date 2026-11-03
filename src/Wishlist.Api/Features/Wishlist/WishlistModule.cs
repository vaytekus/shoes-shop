using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wishlist.Api.Features.Wishlist.Commands.AddItem;
using Wishlist.Api.Features.Wishlist.Commands.MergeItems;
using Wishlist.Api.Features.Wishlist.Commands.RemoveItem;
using Wishlist.Api.Features.Wishlist.Queries.GetWishlist;

namespace Wishlist.Api.Features.Wishlist;

public static class WishlistModule
{
    public static IEndpointRouteBuilder MapWishlist(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/wishlist")
            .WithTags("Wishlist");

        group.MapGet("/{customerId}", GetWishlistEndpoint.Handle)
            .WithName("GetWishlist")
            .WithSummary("GetWishlist");

        group.MapPost("/{customerId}/items", AddItemEndpoint.Handle)
            .WithName("AddWishlistItem")
            .WithSummary("AddWishlistItem");

        group.MapDelete("/{customerId}/items/{productId}", RemoveItemEndpoint.Handle)
            .WithName("RemoveWishlistItem")
            .WithSummary("RemoveWishlistItem");

        group.MapPost("/{customerId}/merge", MergeItemsEndpoint.Handle)
            .WithName("MergeWishlistItems")
            .WithSummary("MergeWishlistItems");

        return app;
    }
}
