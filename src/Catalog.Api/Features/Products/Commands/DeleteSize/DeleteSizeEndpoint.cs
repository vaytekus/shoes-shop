using MediatR;

namespace Catalog.Api.Features.Products.Commands.DeleteSize;

public static class DeleteSizeEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        Guid variantId,
        Guid sizeId,
        IMediator mediator,
        CancellationToken ct)
    {
        await mediator.Send(new DeleteSizeCommand(sizeId), ct);
        return Results.NoContent();
    }
}
