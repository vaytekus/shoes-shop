using Catalog.Api.DTOs.StockCheck;
using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Products.Queries.CheckStock;

public class CheckStockHandler(CatalogDbContext db) : IRequestHandler<CheckStockQuery, StockCheckResult>
{
    public async Task<StockCheckResult> Handle(CheckStockQuery request, CancellationToken ct)
    {
        var unavailable = new List<OutOfStockItem>();

        foreach (var item in request.Items)
        {
            var stock = await db.ProductSizes
                .Include(x => x.Variant)
                .Where(s => s.Variant.ProductId == item.ProductId && s.Size == item.Size)
                .Select(s => s.StockQuantity)
                .FirstOrDefaultAsync(ct);

            if (stock < item.Quantity)
            {
                unavailable.Add(new OutOfStockItem(item.ProductId, item.Size, stock, item.Quantity));
            }
        }

        return new StockCheckResult(unavailable.Count == 0, unavailable);
    }
}
