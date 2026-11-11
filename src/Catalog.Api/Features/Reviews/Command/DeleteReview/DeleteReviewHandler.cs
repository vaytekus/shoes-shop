using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Reviews.Command.DeleteReview;

public class DeleteReviewHandler(CatalogDbContext db) : IRequestHandler<DeleteReviewCommand>
{
    public async Task Handle(DeleteReviewCommand request, CancellationToken ct)
    {
        await db.ProductReviews
            .Where(r => r.Id == request.ReviewId)
            .ExecuteDeleteAsync(ct);
    }
}
