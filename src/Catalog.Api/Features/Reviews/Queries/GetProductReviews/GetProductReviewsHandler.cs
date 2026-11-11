using Catalog.Api.Domain;
using Catalog.Api.DTOs;
using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Reviews.Queries.GetProductReviews;

public class GetProductReviewsHandler(CatalogDbContext db) : IRequestHandler<GetProductReviewsQuery, ProductReviewResult>
{
    public async Task<ProductReviewResult> Handle(GetProductReviewsQuery request, CancellationToken ct)
    {
        var reviews = await db.ProductReviews
            .Where(r => r.ProductId == request.ProductId && r.Status == ReviewStatus.Approved)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new ReviewDto(r.Id, r.CustomerName, r.Rating, r.Text, r.CreatedAt))
            .ToListAsync(ct);

        var averageRating = reviews.Count > 0 ? Math.Round(reviews.Average(r => r.Rating), 1) : 0;

        return new ProductReviewResult(reviews, averageRating, reviews.Count);
    }
}
