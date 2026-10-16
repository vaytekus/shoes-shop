using MediatR;

namespace Catalog.Api.Features.Products.Commands.DeleteVariant;

public static class DeleteVariantEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        Guid variantId,
        IMediator mediator,
        CancellationToken ct)
    {
        await mediator.Send(new DeleteVariantCommand(variantId), ct);
        return Results.NoContent();
    }
}
