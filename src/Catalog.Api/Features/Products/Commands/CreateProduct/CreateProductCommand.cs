using MediatR;

namespace Catalog.Api.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    string? Brand,
    string? Sku,
    int? DiscountPercent,
    bool IsNew,
    string? UpperMaterial,
    string? LiningMaterial,
    string? InsoleMaterial,
    string? SoleMaterial,
    string? Width,
    string? Season,
    string? HeelHeight,
    string? Purpose,
    string? Style,
    string? BrandCountry,
    string? ManufactureCountry,
    Guid? CategoryId) : IRequest<Guid>;
