using Basket.Api.DTOs;
using MediatR;

namespace Basket.Api.Features.Basket.Commands.UpdateItem;

public static class UpdateItemEndpoint
{
    public static async Task<IResult> Handle(
        IMediator mediator,
        string customerId,
        Guid productId,
        UpdateItemRequest request,
        CancellationToken ct)
    {
        var command = new UpdateItemCommand(
            customerId,
            productId,
            request.ProductName,
            request.Price,
            request.Quantity,
            request.ImageUrl);

        var result = await mediator.Send(command, ct);
        return Results.Ok(result);
    }
}
