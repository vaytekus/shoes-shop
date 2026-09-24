namespace Order.Api.DTOs;

public record OrderItemDto(
    Guid ProductId,
    string ProductName,
    decimal Price,
    int Quantity,
    string ImageUrl);
