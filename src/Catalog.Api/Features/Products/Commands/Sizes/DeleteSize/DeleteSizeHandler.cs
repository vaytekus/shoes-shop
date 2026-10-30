using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Products.Commands.Sizes.DeleteSize;

public class DeleteSizeHandler(CatalogDbContext db) : IRequestHandler<DeleteSizeCommand, bool>
{
    public async Task<bool> Handle(DeleteSizeCommand request, CancellationToken ct)
    {
        var rows = await db.ProductSizes
            .Where(s => s.Id == request.SizeId)
            .ExecuteDeleteAsync(ct);

        return rows > 0;
    }
}
