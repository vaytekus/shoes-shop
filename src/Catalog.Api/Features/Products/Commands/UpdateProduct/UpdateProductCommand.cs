using MediatR;

namespace Catalog.Api.Features.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string? Brand,
    string? Gender,
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
    Guid? CategoryId) : IRequest;
