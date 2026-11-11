using MediatR;

namespace Catalog.Api.Features.Reviews.Command.DeleteReview;

public static class DeleteReviewEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        ISender sender,
        CancellationToken ct)
    {
        await sender.Send(new DeleteReviewCommand(id), ct);
        return Results.NoContent();
    }
}
