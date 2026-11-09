using MediatR;

namespace Basket.Api.Features.Basket.Commands.RemoveItem;

public static class RemoveItemEndpoint
{
    public static async Task<IResult> Handle(
        IMediator mediator,
        string customerId,
        Guid productId,
        string size,
        CancellationToken ct)
    {
        await mediator.Send(new RemoveItemCommand(customerId, productId, size), ct);
        return Results.NoContent();
    }
}
