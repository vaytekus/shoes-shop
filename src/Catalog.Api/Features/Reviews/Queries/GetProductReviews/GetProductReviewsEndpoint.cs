using MediatR;

namespace Catalog.Api.Features.Reviews.Queries.GetProductReviews;

public static class GetProductReviewsEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        ISender sender,
        CancellationToken ct)
    {
        var result = await sender.Send(new GetProductReviewsQuery(id), ct);
        return Results.Ok(result);
    }
}
