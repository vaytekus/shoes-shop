using Catalog.Api.Domain;
using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Products.Commands.CreateVariant;

public class CreateVariantHandler(CatalogDbContext db) : IRequestHandler<CreateVariantCommand, Guid>
{
    public async Task<Guid> Handle(CreateVariantCommand request, CancellationToken ct)
    {
        var productExists = await db.Products.AnyAsync(p => p.Id == request.ProductId, ct);

        if (!productExists)
        {
            throw new KeyNotFoundException($"Product {request.ProductId} not found");
        }

        var variant = new ProductVariant
        {
            Id = Guid.NewGuid(), ProductId = request.ProductId, Name = request.Name, Sku = request.Sku, ColorName = request.ColorName, ColorHex = request.ColorHex,
        };

        db.ProductVariants.Add(variant);
        await db.SaveChangesAsync(ct);
        return variant.Id;
    }
}
