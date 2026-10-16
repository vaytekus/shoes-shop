using Catalog.Api.Domain;
using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Products.Commands.AddSize;

public class AddSizeHandler(CatalogDbContext db) : IRequestHandler<AddSizeCommand, Guid>
{
    public async Task<Guid> Handle(AddSizeCommand request, CancellationToken ct)
    {
        var variantExists = await db.ProductVariants.AnyAsync(v => v.Id == request.VariantId, ct);

        if (!variantExists)
        {
            throw new KeyNotFoundException($"Variant {request.VariantId} not found");
        }

        var size = new ProductSize
        {
            Id = Guid.NewGuid(), VariantId = request.VariantId, Size = request.Size, StockQuantity = request.StockQuantity
        };

        db.ProductSizes.Add(size);
        await db.SaveChangesAsync(ct);
        return size.Id;
    }
}
