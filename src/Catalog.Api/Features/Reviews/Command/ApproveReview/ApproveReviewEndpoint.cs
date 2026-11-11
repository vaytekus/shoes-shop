using MediatR;

namespace Catalog.Api.Features.Reviews.Command.ApproveReview;

public static class ApproveReviewEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        ISender sender,
        CancellationToken ct)
    {
        await sender.Send(new ApproveReviewCommand(id), ct);
        return Results.NoContent();
    }
}
