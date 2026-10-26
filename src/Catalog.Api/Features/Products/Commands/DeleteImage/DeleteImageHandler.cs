using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Products.Commands.DeleteImage;

public class DeleteImageHandler(CatalogDbContext db, BlobStorageService blobStorage) : IRequestHandler<DeleteImageCommand>
{
    public async Task Handle(DeleteImageCommand request, CancellationToken ct)
    {
        var images = await db.ProductImages
            .Where(i => i.VariantId == request.VariantId && request.Urls.Contains(i.Url))
            .ToListAsync(ct);

        await Task.WhenAll(images.Select(i => blobStorage.DeleteAsync(i.Url, ct)));

        db.ProductImages.RemoveRange(images);
        await db.SaveChangesAsync(ct);
    }
}
