using MediatR;

namespace Catalog.Api.Features.Products.Queries.SearchProducts;

public static class SearchProductsEndpoint
{
    public static async Task<IResult> Handle(
        string? q,
        string? brand,
        string? season,
        string? size,
        string? section,
        string? purpose,
        string? category,
        int page = 1,
        int pageSize = 20,
        IMediator mediator = null!,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(
        new SearchProductsQuery(q, brand, season, size, section, purpose, category, page, pageSize), ct);
        return Results.Ok(result);
    }
}
