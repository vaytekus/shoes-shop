using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Products.Commands.DeleteSize;

public class DeleteSizeHandler(CatalogDbContext db) : IRequestHandler<DeleteSizeCommand>
{
    public async Task Handle(DeleteSizeCommand request, CancellationToken ct)
    {
        await db.ProductSizes
            .Where(s => s.Id == request.SizeId)
            .ExecuteDeleteAsync(ct);
    }
}
