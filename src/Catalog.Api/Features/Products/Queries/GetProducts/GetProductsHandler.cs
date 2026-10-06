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
        var query = db.Products
            .Include(p => p.Category)
            .Include(p => p.Images)
            .AsQueryable();

        if (request.CategoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == request.CategoryId.Value);
        }

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new ProductResponse(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.Images.OrderBy(i => i.SortOrder)
                    .Select(i => i.Url),
                p.StockQuantity,
                p.CategoryId,
                p.Category != null ? p.Category.Name : null))
            .ToListAsync(ct);

        return new PagedResult<ProductResponse>(
            items,
            totalCount,
            request.Page,
            request.PageSize);
    }
}
