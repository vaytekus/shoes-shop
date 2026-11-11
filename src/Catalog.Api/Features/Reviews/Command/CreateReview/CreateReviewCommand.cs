using MediatR;

namespace Catalog.Api.Features.Reviews.Command.CreateReview;

public record CreateReviewCommand(
    Guid ProductId,
    string CustomerId,
    string CustomerName,
    int Rating,
    string Text): IRequest<Guid>;
