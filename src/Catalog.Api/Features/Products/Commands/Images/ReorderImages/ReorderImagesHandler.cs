using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Products.Commands.Images.ReorderImages;

public class ReorderImagesHandler(CatalogDbContext db) : IRequestHandler<ReorderImagesCommand, bool>
{
    public async Task<bool> Handle(ReorderImagesCommand request, CancellationToken ct)
    {
        var images = await db.ProductImages
            .Where(i => i.VariantId == request.VariantId)
            .ToListAsync(ct);

        for (int i = 0; i < request.Urls.Count; i++)
        {
            var image = images.FirstOrDefault(img => img.Url == request.Urls[i]);
            if (image is not null)
            {
                image.SortOrder = i;
            }
        }

        await db.SaveChangesAsync(ct);
        return true;
    }
}
