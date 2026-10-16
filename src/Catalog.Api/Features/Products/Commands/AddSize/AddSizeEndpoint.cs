using Catalog.Api.DTOs;
using MediatR;

namespace Catalog.Api.Features.Products.Commands.AddSize;

public static class AddSizeEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        Guid variantId,
        AddSizeRequest request,
        IMediator mediator,
            CancellationToken ct)
    {
        var sizeId = await mediator.Send(new AddSizeCommand(variantId, request.Size, request.StockQuantity), ct);

        return Results.Created($"/api/products/{id}/variants/{variantId}/sizes/{sizeId}", sizeId);
    }
}
