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
            .Where(p => p.Id == request.Id)
            .Select(p => new ProductResponse(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.ImageUrl,
                p.StockQuantity
            ))
            .FirstOrDefaultAsync(ct);
    }
}
