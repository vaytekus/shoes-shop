using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Wishlist.Api.Features.Wishlist.Queries.GetWishlist;

public static class GetWishlistEndpoint
{
    public static async Task<IResult> Handle(
        IMediator mediator,
        string customerId,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetWishlistQuery(customerId), ct);
        return Results.Ok(result);
    }
}
