using MediatR;

namespace Catalog.Api.Features.Products.Queries.GetProducts;

public static class GetProductsEndpoint
{
    public static async Task<IResult> Handle(
        IMediator mediator,
        CancellationToken ct,
        int page = 1,
        int pageSize = 10)
    {
        var result = await mediator.Send(new GetProductsQuery(page, pageSize), ct);
        return Results.Ok(result);
    }
}
