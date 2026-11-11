using Catalog.Api.Domain;

namespace Catalog.Api.DTOs;

public record AdminReviewDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    string CustomerName,
    int Rating,
    string Text,
    DateTime CreatedAt,
    ReviewStatus Status);
