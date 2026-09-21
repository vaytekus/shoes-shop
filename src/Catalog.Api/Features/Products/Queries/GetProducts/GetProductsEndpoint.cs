using MediatR;

namespace Catalog.Api.Features.Products.Queries.GetProducts;

public static class GetProductsEndpoint
{
    public static async Task<IResult> Handle(IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(new GetProductsQuery(), ct);
        return Results.Ok(result);
    }
}
