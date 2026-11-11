using Catalog.Api.Domain;
using Catalog.Api.Infrastructure;
using MediatR;

namespace Catalog.Api.Features.Reviews.Command.ApproveReview;

public class ApproveReviewHandler(CatalogDbContext db) : IRequestHandler<ApproveReviewCommand>
{
    public async Task Handle(ApproveReviewCommand request, CancellationToken ct)
    {
        var review = await db.ProductReviews.FindAsync([request.ReviewId], ct)
            ?? throw new KeyNotFoundException($"Review {request.ReviewId} not found");

        review.Status = ReviewStatus.Approved;
        await db.SaveChangesAsync(ct);
    }
}
