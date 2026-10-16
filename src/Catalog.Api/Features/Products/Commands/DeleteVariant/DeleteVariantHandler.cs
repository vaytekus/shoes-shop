using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Products.Commands.DeleteVariant;

public class DeleteVariantHandler(CatalogDbContext db) : IRequestHandler<DeleteVariantCommand>
{
    public async Task Handle(DeleteVariantCommand request, CancellationToken ct)
    {
        await db.ProductVariants
            .Where(v => v.Id == request.VariantId)
            .ExecuteDeleteAsync(ct);
    }
}
