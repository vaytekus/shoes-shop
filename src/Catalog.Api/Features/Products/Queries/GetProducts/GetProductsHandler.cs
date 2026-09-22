using Catalog.Api.DTOs;
using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoesShop.Shared.Pagination;

namespace Catalog.Api.Features.Products.Queries.GetProducts;

public class GetProductsHandler(CatalogDbContext db) : IRequestHandler<GetProductsQuery, PagedResult<ProductResponse>>
{
    public async Task<PagedResult<ProductResponse>> Handle(GetProductsQuery request, CancellationToken ct)
    {
        var totalCount = await db.Products.CountAsync(ct);

        var items = await db.Products
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new ProductResponse(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.ImageUrl,
                p.StockQuantity))
            .ToListAsync(ct);

        return new PagedResult<ProductResponse>(
            items,
            totalCount,
            request.Page,
            request.PageSize);
    }
}
