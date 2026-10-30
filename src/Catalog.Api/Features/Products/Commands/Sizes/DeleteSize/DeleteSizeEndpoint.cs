using MediatR;

namespace Catalog.Api.Features.Products.Commands.Sizes.DeleteSize;

public static class DeleteSizeEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        Guid variantId,
        Guid sizeId,
        IMediator mediator,
        CancellationToken ct)
    {
        var found = await mediator.Send(new DeleteSizeCommand(sizeId), ct);
        return found ? Results.NoContent() : Results.NotFound();
    }
}
