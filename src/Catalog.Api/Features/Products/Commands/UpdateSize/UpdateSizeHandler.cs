using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Products.Commands.UpdateSize;

public class UpdateSizeHandler(CatalogDbContext db) : IRequestHandler<UpdateSizeCommand>
{
    public async Task Handle(UpdateSizeCommand request, CancellationToken ct)
    {
        var size = await db.ProductSizes
            .FirstOrDefaultAsync(s => s.Id == request.SizeId, ct);

        if (size is null)
            throw new KeyNotFoundException($"Size {request.SizeId} not found");

        size.Size = request.Size;
        size.StockQuantity = request.StockQuantity;
        await db.SaveChangesAsync(ct);
    }
}
