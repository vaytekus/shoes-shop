namespace Catalog.Api.DTOs;

public record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string ImageUrl,
    int StockQuantity);
