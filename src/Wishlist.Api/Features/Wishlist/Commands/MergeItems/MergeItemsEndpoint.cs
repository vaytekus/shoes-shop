using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Wishlist.Api.DTOs;

namespace Wishlist.Api.Features.Wishlist.Commands.MergeItems;

public static class MergeItemsEndpoint
{
    public static async Task<IResult> Handle(
        IMediator mediator,
        string customerId,
        MergeItemsRequest request,
        CancellationToken ct)
    {
        var command = new MergeItemsCommand(customerId, request.Items);
        var result = await mediator.Send(command, ct);
        return Results.Ok(result);
    }
}
