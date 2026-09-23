namespace Basket.Api.DTOs;

public record AddItemRequest(
    Guid ProductId,
    string ProductName,
    decimal Price,
    int Quantity,
    string ImageUrl);
