using Catalog.Api.Domain;
using Catalog.Api.DTOs;
using MediatR;

namespace Catalog.Api.Features.Reviews.Queries.GetReviews;

public record GetReviewsQuery(ReviewStatus? Status) : IRequest<IReadOnlyList<AdminReviewDto>>;
