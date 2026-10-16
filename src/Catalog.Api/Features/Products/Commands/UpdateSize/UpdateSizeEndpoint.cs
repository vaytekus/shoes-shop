using Catalog.Api.DTOs;
using MediatR;

namespace Catalog.Api.Features.Products.Commands.UpdateSize;

public static class UpdateSizeEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        Guid variantId,
        Guid sizeId,
        AddSizeRequest request,
        IMediator mediator,
        CancellationToken ct)
    {
        await mediator.Send(new UpdateSizeCommand(sizeId, request.Size, request.StockQuantity), ct);
        return Results.NoContent();
    }
}
