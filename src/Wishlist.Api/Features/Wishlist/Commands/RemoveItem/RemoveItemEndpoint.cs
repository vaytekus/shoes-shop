using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Wishlist.Api.Features.Wishlist.Commands.RemoveItem;

public static class RemoveItemEndpoint
{
    public static async Task<IResult> Handle(
        IMediator mediator,
        string customerId,
        Guid productId,
        CancellationToken ct)
    {
        await mediator.Send(new RemoveItemCommand(customerId, productId), ct);
        return Results.NoContent();
    }
}
