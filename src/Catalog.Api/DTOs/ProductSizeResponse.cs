namespace Catalog.Api.DTOs;

public record ProductSizeResponse(
    Guid Id,
    string Size,
    int StockQuantity);
