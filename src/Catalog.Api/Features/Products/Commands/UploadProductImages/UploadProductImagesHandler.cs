using Catalog.Api.Domain;
using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Products.Commands.UploadProductImages;

public class UploadProductImagesHandler(CatalogDbContext db) : IRequestHandler<UploadProductImagesCommand, bool>
{
    public async Task<bool> Handle(UploadProductImagesCommand request, CancellationToken ct)
    {
        var variant = await db.ProductVariants
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == request.VariantId, ct);

        if (variant is null)
        {
            return false;
        }

        var nextOrder = variant.Images.Count;
        var images = request.Urls.Select((url, i) => new ProductImage
        {
            Id = Guid.NewGuid(), VariantId = request.VariantId, Url = url, SortOrder = nextOrder + i
        });

        db.ProductImages.AddRange(images);
        await db.SaveChangesAsync(ct);
        return true;
    }
}
