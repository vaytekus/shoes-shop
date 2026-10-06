namespace Catalog.Api.DTOs;

public record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    IEnumerable<string> ImageUrls,
    int StockQuantity,
    Guid? CategoryId,
    string? CategoryName);
