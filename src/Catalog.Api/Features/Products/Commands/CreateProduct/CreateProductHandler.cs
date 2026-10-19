using Catalog.Api.Domain;
using Catalog.Api.Infrastructure;
using MediatR;

namespace Catalog.Api.Features.Products.Commands.CreateProduct;

public class CreateProductHandler(CatalogDbContext db) : IRequestHandler<CreateProductCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Brand = request.Brand,
            Gender = request.Gender,
            Sku = request.Sku,
            DiscountPercent = request.DiscountPercent,
            IsNew = request.IsNew,
            UpperMaterial = request.UpperMaterial,
            LiningMaterial = request.LiningMaterial,
            InsoleMaterial = request.InsoleMaterial,
            SoleMaterial = request.SoleMaterial,
            Width = request.Width,
            Season = request.Season,
            HeelHeight = request.HeelHeight,
            Purpose = request.Purpose,
            Style = request.Style,
            BrandCountry = request.BrandCountry,
            ManufactureCountry = request.ManufactureCountry,
            CategoryId = request.CategoryId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        db.Products.Add(product);
        await db.SaveChangesAsync(ct);
        return product.Id;
    }
}
