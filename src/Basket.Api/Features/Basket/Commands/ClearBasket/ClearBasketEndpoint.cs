using MediatR;

namespace Basket.Api.Features.Basket.Commands.ClearBasket;

public static class ClearBasketEndpoint
{
    public static async Task<IResult> Handle(
        IMediator mediator,
        string customerId,
        CancellationToken ct)
    {
        await mediator.Send(new ClearBasketCommand(customerId), ct);
        return Results.NoContent();
    }
}
