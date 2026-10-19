namespace Catalog.Api.DTOs;

public record CreateVariantRequest(
    string Name,
    string? Sku,
    string? ColorName,
    string? ColorHex);
