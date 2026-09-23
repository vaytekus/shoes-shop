using MediatR;

namespace Basket.Api.Features.Basket.Commands.RemoveItem;

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
