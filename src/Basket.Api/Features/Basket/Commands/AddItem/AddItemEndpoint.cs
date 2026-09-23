using Basket.Api.DTOs;
using MediatR;

namespace Basket.Api.Features.Basket.Commands.AddItem;

public static class AddItemEndpoint
{
    public static async Task<IResult> Handle(
        IMediator mediator,
        AddItemRequest request,
        string customerId,
        CancellationToken ct)
    {
        var command = new AddItemCommand(
            customerId,
            request.ProductId,
            request.ProductName,
            request.Price,
            request.Quantity,
            request.ImageUrl
        );

        var result = await mediator.Send(command, ct);
        return Results.Ok(result);
    }
}
