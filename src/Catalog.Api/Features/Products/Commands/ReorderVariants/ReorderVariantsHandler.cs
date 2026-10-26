using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Products.Commands.ReorderVariants;

public class ReorderVariantsHandler(CatalogDbContext db) : IRequestHandler<ReorderVariantsCommand>
{
    public async Task Handle(ReorderVariantsCommand request, CancellationToken ct)
    {
        var variants = await db.ProductVariants
            .Where(v => v.ProductId == request.ProductId)
            .ToListAsync();

        for (int i = 0; i < request.VariantIds.Count; i++)
        {
            var variant = variants.FirstOrDefault(v => v.Id == request.VariantIds[i]);

            if (variant is not null)
            {
                variant.SortOrder = i;
            }

        }

        await db.SaveChangesAsync(ct);
    }
}
