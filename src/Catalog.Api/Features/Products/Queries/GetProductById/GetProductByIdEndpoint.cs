using MediatR;

namespace Catalog.Api.Features.Products.Queries.GetProductById;

public static class GetProductByIdEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        IMediator mediator,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetProductByIdQuery(id), ct);

        return result is null
            ? Results.NotFound()
            : Results.Ok(result);
    }
}
