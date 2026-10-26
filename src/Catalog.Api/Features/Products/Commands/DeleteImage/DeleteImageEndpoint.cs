using MediatR;

namespace Catalog.Api.Features.Products.Commands.DeleteImage;

public static class DeleteImageEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        Guid variantId,
        [Microsoft.AspNetCore.Mvc.FromBody] List<string> urls,
        IMediator mediator,
        CancellationToken ct)
    {
        await mediator.Send(new DeleteImageCommand(variantId, urls), ct);
        return Results.Ok();
    }
}
