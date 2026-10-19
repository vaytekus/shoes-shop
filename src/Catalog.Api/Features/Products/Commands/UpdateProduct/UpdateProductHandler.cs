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
        product.Brand = request.Brand;
        product.Gender = request.Gender;
        product.Sku = request.Sku;
        product.DiscountPercent = request.DiscountPercent;
        product.IsNew = request.IsNew;
        product.UpperMaterial = request.UpperMaterial;
        product.LiningMaterial = request.LiningMaterial;
        product.InsoleMaterial = request.InsoleMaterial;
        product.SoleMaterial = request.SoleMaterial;
        product.Width = request.Width;
        product.Season = request.Season;
        product.HeelHeight = request.HeelHeight;
        product.Purpose = request.Purpose;
        product.Style = request.Style;
        product.BrandCountry = request.BrandCountry;
        product.ManufactureCountry = request.ManufactureCountry;
        product.CategoryId = request.CategoryId;
        product.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);
    }
}
