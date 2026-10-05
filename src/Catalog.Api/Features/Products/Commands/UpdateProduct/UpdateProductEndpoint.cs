using Catalog.Api.DTOs;
using MediatR;

namespace Catalog.Api.Features.Products.Commands.UpdateProduct;

public static class UpdateProductEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        UpdateProductRequest request,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new UpdateProductCommand(
            id,
            request.Name,
            request.Description,
            request.Price,
            request.ImageUrl,
            request.StockQuantity,
            request.CategoryId);

        await mediator.Send(command, ct);
        return Results.NoContent();
    }
}
