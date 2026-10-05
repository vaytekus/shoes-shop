using MediatR;

namespace Catalog.Api.Features.Categories.Commands.CreateCategory;

public static class CreateCategoryEndpoints
{
    public static async Task<IResult> Handle(
        ISender sender,
        CreateCategoryCommand command,
        CancellationToken ct)
    {
        var result = await sender.Send(command, ct);
        return Results.Created($"/api/categories/{result.Id}", result);
    }
}
