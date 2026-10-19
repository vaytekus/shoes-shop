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
        request.Brand,
        request.Gender,
        request.Sku,
        request.DiscountPercent,
        request.IsNew,
        request.UpperMaterial,
        request.LiningMaterial,
        request.InsoleMaterial,
        request.SoleMaterial,
        request.Width,
        request.Season,
        request.HeelHeight,
        request.Purpose,
        request.Style,
        request.BrandCountry,
        request.ManufactureCountry,
        request.CategoryId);

        await mediator.Send(command, ct);
        return Results.NoContent();
    }
}
