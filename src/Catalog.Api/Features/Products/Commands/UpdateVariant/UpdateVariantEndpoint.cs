using Catalog.Api.DTOs;
using MediatR;

namespace Catalog.Api.Features.Products.Commands.UpdateVariant;

public static class UpdateVariantEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        Guid variantId,
        CreateVariantRequest request,
        IMediator mediator,
        CancellationToken ct)
    {
        await mediator.Send(new UpdateVariantCommand(variantId, request.Name, request.Sku, request.ColorName, request.ColorHex), ct);
        return Results.NoContent();
    }
}
