using MediatR;

namespace Catalog.Api.Features.Products.Commands.Images.DeleteImage;

public static class DeleteImageEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        Guid variantId,
        [Microsoft.AspNetCore.Mvc.FromBody] List<string> urls,
        IMediator mediator,
        CancellationToken ct)
    {
        var found = await mediator.Send(new DeleteImageCommand(variantId, urls), ct);
        return found ? Results.Ok(): Results.NotFound();
    }
}
