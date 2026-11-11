using Catalog.Api.Domain;
using Catalog.Api.Infrastructure;
using MediatR;

namespace Catalog.Api.Features.Reviews.Command.RejectReview;

public class RejectReviewHandler(CatalogDbContext db) : IRequestHandler<RejectReviewCommand>
{
    public async Task Handle(RejectReviewCommand request, CancellationToken ct)
    {
        var review = await db.ProductReviews.FindAsync([request.ReviewId], ct)
            ?? throw new KeyNotFoundException($"Review {request.ReviewId} not found");

        review.Status = ReviewStatus.Rejected;
        await db.SaveChangesAsync(ct);
    }
}
