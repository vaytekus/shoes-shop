using Catalog.Api.DTOs;
using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Reviews.Queries.GetReviews;

public class GetReviewsHandler(CatalogDbContext db) : IRequestHandler<GetReviewsQuery, IReadOnlyList<AdminReviewDto>>
{
    public async Task<IReadOnlyList<AdminReviewDto>> Handle(GetReviewsQuery request, CancellationToken ct)
    {
        var query = db.ProductReviews
            .Include(r => r.Product)
            .AsQueryable();

        if (request.Status.HasValue)
        {
            query = query.Where(r => r.Status == request.Status.Value);
        }

        return await query
            .OrderBy(r => r.CreatedAt)
            .Select(r => new AdminReviewDto(
                r.Id,
                r.ProductId,
                r.Product.Name,
                r.CustomerName,
                r.Rating,
                r.Text,
                r.CreatedAt,
                r.Status))
            .ToListAsync(ct);
    }
}
