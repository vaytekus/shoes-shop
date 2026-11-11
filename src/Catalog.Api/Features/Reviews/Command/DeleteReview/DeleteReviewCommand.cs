using MediatR;

namespace Catalog.Api.Features.Reviews.Command.DeleteReview;

public record DeleteReviewCommand(Guid ReviewId) : IRequest;
