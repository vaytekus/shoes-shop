using System.Security.Claims;
using Catalog.Api.DTOs;
using MediatR;

namespace Catalog.Api.Features.Reviews.Command.CreateReview;

public static class CreateReviewEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        IMediator mediator,
        CreateReviewRequest request,
        ClaimsPrincipal user,
        CancellationToken ct)
    {
        var customerId = user.FindFirst("sub")?.Value;
        if (customerId is null)
        {
            return Results.Unauthorized();
        }

        var firstName = user.FindFirst("given_name")?.Value ?? "";
        var lastName = user.FindFirst("family_name")?.Value ?? "";
        var customerName = $"{firstName} {lastName}".Trim();

        var reviewId = await mediator.Send(new CreateReviewCommand(id, customerId, customerName, request.Rating, request.Text), ct);

        return Results.Created($"/api/reviews/{reviewId}", new
        {
            Id = reviewId
        });
    }
}
