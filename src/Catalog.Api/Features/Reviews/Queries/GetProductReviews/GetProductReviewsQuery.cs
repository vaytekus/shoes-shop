using Catalog.Api.DTOs;
using MediatR;

namespace Catalog.Api.Features.Reviews.Queries.GetProductReviews;

public record GetProductReviewsQuery(Guid ProductId) : IRequest<ProductReviewResult>;

