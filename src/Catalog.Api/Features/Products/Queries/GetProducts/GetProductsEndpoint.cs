using MediatR;

namespace Catalog.Api.Features.Products.Queries.GetProducts;

public static class GetProductsEndpoint
{
    public static async Task<IResult> Handle(
        ISender sender,
        CancellationToken ct,
        int page = 1,
        int pageSize = 10,
        Guid? categoryId = null,
        string? gender = null,
        bool? hasDiscount = null)
    {
        var result = await sender.Send(new GetProductsQuery(page, pageSize, categoryId, gender, hasDiscount), ct);
        return Results.Ok(result);
    }
}
