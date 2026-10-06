using Catalog.Api.Domain;
using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Products.Commands.UploadProductImages;

public class UploadProductImagesHandler(CatalogDbContext db) : IRequestHandler<UploadProductImagesCommand, bool>
{
    public async Task<bool> Handle(UploadProductImagesCommand request, CancellationToken ct)
    {
        var product = await db.Products
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, ct);

        if (product is null)
        {
            return false;
        }

        var nextOrder = product.Images.Count;
        var images = request.Urls.Select((url, i) => new ProductImage
        {
            Id = Guid.NewGuid(), ProductId = request.ProductId, Url = url, SortOrder = nextOrder + i
        });

        db.ProductImages.AddRange(images);
        await db.SaveChangesAsync(ct);
        return true;
    }
}
