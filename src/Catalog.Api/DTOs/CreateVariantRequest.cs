namespace Catalog.Api.DTOs;

public record CreateVariantRequest(
    string ColorName,
    string? ColorHex);
