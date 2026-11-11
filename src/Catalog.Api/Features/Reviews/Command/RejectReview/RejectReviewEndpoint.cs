using MediatR;

namespace Catalog.Api.Features.Reviews.Command.RejectReview;

public static class RejectReviewEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        ISender sender,
        CancellationToken ct)
    {
        await sender.Send(new RejectReviewCommand(id), ct);
        return Results.NoContent();
    }
}
