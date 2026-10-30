using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Products.Commands.Variants.DeleteVariant;

public class DeleteVariantHandler(CatalogDbContext db) : IRequestHandler<DeleteVariantCommand, bool>
{
    public async Task<bool> Handle(DeleteVariantCommand request, CancellationToken ct)
    {
        var rows = await db.ProductVariants
            .Where(v => v.Id == request.VariantId)
            .ExecuteDeleteAsync(ct);

        return rows > 0;
    }
}
