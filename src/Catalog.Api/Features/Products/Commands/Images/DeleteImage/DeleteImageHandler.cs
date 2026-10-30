using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Products.Commands.Images.DeleteImage;

public class DeleteImageHandler(CatalogDbContext db, BlobStorageService blobStorage) : IRequestHandler<DeleteImageCommand, bool>
{
    public async Task<bool> Handle(DeleteImageCommand request, CancellationToken ct)
    {
        var images = await db.ProductImages
            .Where(i => i.VariantId == request.VariantId && request.Urls.Contains(i.Url))
            .ToListAsync(ct);

        if (images.Count == 0)
        {
            return false;
        }

        await Task.WhenAll(images.Select(i => blobStorage.DeleteAsync(i.Url, ct)));

        db.ProductImages.RemoveRange(images);
        await db.SaveChangesAsync(ct);
        return true;
    }
}
