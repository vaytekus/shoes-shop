using MediatR;

namespace Catalog.Api.Features.Reviews.Command.ApproveReview;

public record ApproveReviewCommand(Guid ReviewId) : IRequest;
