using Catalog.Api.DTOs;
using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Products.Queries.GetProducts;

public class GetProductsHandler(CatalogDbContext db) : IRequestHandler<GetProductsQuery, List<ProductResponse>>
{
    public async Task<List<ProductResponse>> Handle(GetProductsQuery request, CancellationToken ct)
    {
        return await db.Products
            .Select(p => new ProductResponse(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.ImageUrl,
                p.StockQuantity))
            .ToListAsync(ct);
    }
}
