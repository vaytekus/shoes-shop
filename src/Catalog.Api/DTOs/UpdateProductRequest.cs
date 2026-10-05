namespace Catalog.Api.DTOs;

public record UpdateProductRequest(
    string Name,
    string Description,
    decimal Price,
    string ImageUrl,
    int StockQuantity,
    Guid? CategoryId);
