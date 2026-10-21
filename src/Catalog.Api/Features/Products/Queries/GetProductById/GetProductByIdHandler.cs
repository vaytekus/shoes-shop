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
            .Include(p => p.Variants)
                .ThenInclude(v => v.Images)
            .Include(p => p.Variants)
                .ThenInclude(v => v.Sizes)
            .Where(p => p.Id == request.Id)
            .Select(p => new ProductResponse(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.Brand,
                p.Section,
                p.Sku,
                p.DiscountPercent,
                p.IsNew,
                p.UpperMaterial,
                p.LiningMaterial,
                p.InsoleMaterial,
                p.SoleMaterial,
                p.Width,
                p.Season,
                p.HeelHeight,
                p.Purpose,
                p.Style,
                p.BrandCountry,
                p.ManufactureCountry,
                p.CategoryId,
                p.Category != null ? p.Category.Name : null,
                p.Variants.Select(v => new ProductVariantResponse(
                    v.Id,
                    v.Name,
                    v.Sku,
                    v.ColorName,
                    v.ColorHex,
                    v.Images.OrderBy(i => i.SortOrder).Select(i => i.Url),
                    v.Sizes.Select(s => new ProductSizeResponse(s.Id, s.Size, s.StockQuantity))
                ))
            ))
            .FirstOrDefaultAsync(ct);
    }
}
