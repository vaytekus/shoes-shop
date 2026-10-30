using Catalog.Api.Infrastructure;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoesShop.Shared.Events;

namespace Catalog.Api.Features.Products.Commands.Products.DeleteProduct;

public class DeleteProductHandler(CatalogDbContext db, IPublishEndpoint publish) : IRequestHandler<DeleteProductCommand, bool>
{
    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken ct)
    {
        var product = await db.Products
            .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (product is null)
        {
            return false;
        }

        db.Products.Remove(product);
        await db.SaveChangesAsync(ct);
        await publish.Publish(new ProductDeletedEvent(request.Id), ct);
        return true;
    }
}
