using MediatR;

namespace Catalog.Api.Features.Reviews.Command.RejectReview;

public record RejectReviewCommand(Guid ReviewId) : IRequest;
