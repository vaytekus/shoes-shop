using MediatR;

namespace Catalog.Api.Features.Products.Commands.Products.DeleteProduct;

public static class DeleteProductEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        IMediator mediator,
        CancellationToken ct)
    {
        var found = await mediator.Send(new DeleteProductCommand(id), ct);
        return found ? Results.NoContent() : Results.NotFound();
    }
}
