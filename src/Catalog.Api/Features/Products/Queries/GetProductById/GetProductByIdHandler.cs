using Catalog.Api.DTOs;
using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Products.Queries.GetProductById;

public class GetProductByIdHandler(CatalogDbContext db) : IRequestHandler<GetProductByIdQuery, ProductResponse?>
{
    public async Task<ProductResponse?> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        return await db.Products
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => p.Id == request.Id)
            .Select(p => new ProductResponse(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.Images.OrderBy(i => i.SortOrder)
                    .Select(i => i.Url),
                p.StockQuantity,
                p.CategoryId,
                p.Category != null ? p.Category.Name : null
            ))
            .FirstOrDefaultAsync(ct);
    }
}
