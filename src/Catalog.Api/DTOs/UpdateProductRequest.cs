namespace Catalog.Api.DTOs;

public record UpdateProductRequest(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    Guid? CategoryId);
