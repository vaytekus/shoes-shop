using MediatR;

namespace Catalog.Api.Features.Products.Commands.ReindexProducts;

public static class ReindexProductsEndpoint
{
    public static async Task<IResult> Handle(
        ISender sender,
        CancellationToken ct)
    {
        var count = await sender.Send(new ReindexProductsCommand(), ct);
        return Results.Ok(new {reindexed  = count});
    }
}
