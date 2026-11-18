using Catalog.Api.DTOs.StockCheck;
using MediatR;

namespace Catalog.Api.Features.Products.Queries.CheckStock;

public static class StockCheckEndpoint
{
    public static async Task<IResult> Handle(
        ISender sender,
        List<StockCheckItem> items,
        CancellationToken ct
    )
    {
        var result = await sender.Send(new CheckStockQuery(items), ct);
        return Results.Ok(result);
    }
}
