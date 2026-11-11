namespace Catalog.Api.DTOs;

public record ReviewDto(
    Guid Id,
    string CustomerName,
    int Rating,
    string Text,
    DateTime CreatedAt);
