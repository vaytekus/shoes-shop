using Catalog.Api.Features.Reviews.Command.ApproveReview;
using Catalog.Api.Features.Reviews.Command.CreateReview;
using Catalog.Api.Features.Reviews.Command.DeleteReview;
using Catalog.Api.Features.Reviews.Command.RejectReview;
using Catalog.Api.Features.Reviews.Queries.GetProductReviews;
using Catalog.Api.Features.Reviews.Queries.GetReviews;

namespace Catalog.Api.Features.Reviews;

public static class ReviewModule
{
    public static IEndpointRouteBuilder MapReviews(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products/{id:guid}/reviews", GetProductReviewsEndpoint.Handle)
            .WithTags("Reviews")
            .WithName("GetProductReviews")
            .WithSummary("GetProductReviews");

        app.MapPost("/api/products/{id:guid}/reviews", CreateReviewEndpoint.Handle)
            .WithTags("Reviews")
            .WithName("CreateReview")
            .WithSummary("CreateReview")
            .RequireAuthorization();

        app.MapGet("/api/reviews", GetReviewsEndpoint.Handle)
            .WithTags("Reviews")
            .WithName("GetReviews")
            .WithSummary("GetReviews")
            .RequireAuthorization("AdminOnly");

        app.MapPut("/api/reviews/{id:guid}/approve", ApproveReviewEndpoint.Handle)
            .WithTags("Reviews")
            .WithName("ApproveReview")
            .WithSummary("ApproveReview")
            .RequireAuthorization("AdminOnly");

        app.MapPut("/api/reviews/{id:guid}/reject", RejectReviewEndpoint.Handle)
            .WithTags("Reviews")
            .WithName("RejectReview")
            .WithSummary("RejectReview")
            .RequireAuthorization("AdminOnly");

        app.MapDelete("/api/reviews/{id:guid}", DeleteReviewEndpoint.Handle)
            .WithTags("Reviews")
            .WithName("DeleteReview")
            .WithSummary("DeleteReview")
            .RequireAuthorization("AdminOnly");

        return app;
    }
}
