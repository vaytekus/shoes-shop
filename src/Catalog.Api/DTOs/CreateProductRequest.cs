namespace Catalog.Api.DTOs;

public record CreateProductRequest(
    string Name,
    string Description,
    decimal Price,
    string ImageUrl,
    int StockQuantity);
