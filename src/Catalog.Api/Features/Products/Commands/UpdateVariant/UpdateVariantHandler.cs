using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Products.Commands.UpdateVariant;

public class UpdateVariantHandler(CatalogDbContext db) : IRequestHandler<UpdateVariantCommand>
{
    public async Task Handle(UpdateVariantCommand request, CancellationToken ct)
    {
        var variant = await db.ProductVariants
            .FirstOrDefaultAsync(v => v.Id == request.VariantId, ct);

        if (variant is null)
            throw new KeyNotFoundException($"Variant {request.VariantId} not found");

        variant.Name = request.Name;
        variant.Sku = request.Sku;
        variant.ColorName = request.ColorName;
        variant.ColorHex = request.ColorHex;
        await db.SaveChangesAsync(ct);
    }
}
