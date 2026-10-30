using MediatR;

namespace Catalog.Api.Features.Products.Commands.Variants.SetPrimaryVariant;

public static class SetPrimaryVariantEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        Guid variantId,
        ISender sender,
        CancellationToken ct)
    {
        var found = await sender.Send(new SetPrimaryVariantCommand(id, variantId), ct);
        return  found
            ? Results.NoContent()
            : Results.NotFound();
    }
}
