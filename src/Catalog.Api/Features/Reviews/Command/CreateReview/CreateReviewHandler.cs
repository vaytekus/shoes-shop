using Catalog.Api.Domain;
using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Reviews.Command.CreateReview;

public class CreateReviewHandler(CatalogDbContext db) : IRequestHandler<CreateReviewCommand, Guid>
{
    public async Task<Guid> Handle(CreateReviewCommand request, CancellationToken ct)
    {
        var productExists = await db.Products.AnyAsync(p => p.Id == request.ProductId, ct);
        if (!productExists)
        {
            throw new KeyNotFoundException($"Product {request.ProductId} not found");
        }

        var alreadyReviewed = await db.ProductReviews
            .AnyAsync(r => r.ProductId == request.ProductId && r.CustomerId == request.CustomerId, ct);

        if (alreadyReviewed)
        {
            throw new InvalidOperationException("You have already reviewed this product");
        }

        var review = new ProductReview
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            CustomerId = request.CustomerId,
            CustomerName = request.CustomerName,
            Rating = request.Rating,
            Text = request.Text,
            CreatedAt = DateTime.UtcNow,
            Status = ReviewStatus.Pending
        };

        db.ProductReviews.Add(review);
        await  db.SaveChangesAsync(ct);
        return review.Id;
    }
}
