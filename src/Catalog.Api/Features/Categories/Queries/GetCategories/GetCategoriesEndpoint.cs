using MediatR;

namespace Catalog.Api.Features.Categories.Queries.GetCategories;

public static class GetCategoriesEndpoint
{
    public static async Task<IResult> Handle(
        ISender sender,
        CancellationToken ct)
    {
        var result = await sender.Send(new GetCategoriesQuery(), ct);
        return Results.Ok(result);
    }
}
