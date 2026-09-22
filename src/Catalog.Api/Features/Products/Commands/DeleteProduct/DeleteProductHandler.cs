using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Products.Commands.DeleteProduct;

public class DeleteProductHandler(CatalogDbContext db) : IRequestHandler<DeleteProductCommand>
{
    public async Task Handle(DeleteProductCommand request, CancellationToken ct)
    {
        var product = await db.Products
            .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (product is null)
        {
            throw new KeyNotFoundException($"Product with id {request.Id} not found");
        }

        db.Products.Remove(product);
        await db.SaveChangesAsync(ct);
    }
}
