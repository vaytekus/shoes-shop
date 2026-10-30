using MediatR;

namespace Catalog.Api.Features.Products.Commands.Variants.DeleteVariant;

public static class DeleteVariantEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        Guid variantId,
        IMediator mediator,
        CancellationToken ct)
    {
        var found = await mediator.Send(new DeleteVariantCommand(variantId), ct);
        return found ? Results.NoContent() : Results.NotFound();
    }
}
