using Catalog.Api.DTOs;
using MediatR;

namespace Catalog.Api.Features.Products.Commands.CreateVariant;

public static class CreateVariantEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        CreateVariantRequest request,
        IMediator mediator,
        CancellationToken ct)
    {
        var variantId = await mediator.Send(new CreateVariantCommand(id, request.ColorName, request.ColorHex), ct);

        return Results.Created($"/api/products/{id}/variants/{variantId}", variantId);
    }
}
