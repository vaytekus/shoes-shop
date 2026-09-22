using Catalog.Api.DTOs;
using MediatR;

namespace Catalog.Api.Features.Products.Commands.CreateProduct;

public static class CreateProductEndpoint
{
    public static async Task<IResult> Handle(
        CreateProductRequest request,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new CreateProductCommand(
            request.Name,
            request.Description,
            request.Price,
            request.ImageUrl,
            request.StockQuantity);

        var id = await mediator.Send(command, ct);
        return Results.Created($"/api/products/{id}", id);
    }
}
