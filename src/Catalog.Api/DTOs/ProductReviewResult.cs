namespace Catalog.Api.DTOs;

public record ProductReviewResult(
    IReadOnlyList<ReviewDto> Reviews,
    double AverageRating,
    int TotalCount);
