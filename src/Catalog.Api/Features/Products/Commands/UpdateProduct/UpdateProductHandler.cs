using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Products.Commands.UpdateProduct;

public class UpdateProductHandler(CatalogDbContext db) : IRequestHandler<UpdateProductCommand>
{
    public async Task Handle(UpdateProductCommand request, CancellationToken ct)
    {
        var product = await db.Products
            .FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (product is null)
        {
            throw new KeyNotFoundException($"Product with id {request.Id} not found");
        }

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.StockQuantity = request.StockQuantity;
        product.CategoryId = request.CategoryId;
        product.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
    }
}
