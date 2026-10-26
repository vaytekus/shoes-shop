using MediatR;

namespace Catalog.Api.Features.Products.Commands.ReorderImages;

public static class ReorderImagesEndpoint
{
    public static async Task<IResult> Handle(
        IMediator mediator,
        Guid id,
        Guid variantId,
        List<string> urls,
        CancellationToken ct)
    {
        await mediator.Send(new ReorderImagesCommand(id, variantId, urls), ct);
        return Results.NoContent();
    }
}
