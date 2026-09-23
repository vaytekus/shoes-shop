namespace Basket.Api.DTOs;

public record UpdateItemRequest(
    string ProductName,
    decimal Price,
    int Quantity,
    string ImageUrl);
