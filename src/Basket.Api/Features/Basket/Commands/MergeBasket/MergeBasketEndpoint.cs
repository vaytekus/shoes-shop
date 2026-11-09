using Basket.Api.DTOs;
using MediatR;

namespace Basket.Api.Features.Basket.Commands.MergeBasket;

public static class MergeBasketEndpoint
{
    public static async Task<IResult> Handle(
        IMediator mediator,
        MergeBasketRequest request,
        string customerId,
        CancellationToken ct)
    {
        var result = await mediator.Send(new MergeBasketCommand(customerId, request.GuestId), ct);
        return Results.Ok(result);
    }
}
