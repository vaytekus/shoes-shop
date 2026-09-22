using MediatR;

namespace Catalog.Api.Features.Products.Commands.DeleteProduct;

public static class DeleteProductEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        IMediator mediator,
        CancellationToken ct)
    {
        await mediator.Send(new DeleteProductCommand(id), ct);
        return Results.NoContent();
    }
}
