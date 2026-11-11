using Catalog.Api.Domain;
using MediatR;

namespace Catalog.Api.Features.Reviews.Queries.GetReviews;

public static class GetReviewsEndpoint
{
    public static async Task<IResult> Handle(
        ReviewStatus? status,
        ISender sender,
        CancellationToken ct)
    {
        var result = await sender.Send(new GetReviewsQuery(status), ct);
        return Results.Ok(result);
    }
}
