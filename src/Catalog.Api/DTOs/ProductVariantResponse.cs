namespace Catalog.Api.DTOs;

public record ProductVariantResponse(
    Guid Id,
    string ColorName,
    string? ColorHex,
    IEnumerable<string> ImageUrls,
    IEnumerable<ProductSizeResponse> Sizes);
