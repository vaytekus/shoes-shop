using MediatR;

namespace Catalog.Api.Features.Categories.Commands.DeleteCategory;

public static class DeleteCategoryEndpoints
{
    public static async Task<IResult> Handle(
        ISender sender,
        Guid id,
        CancellationToken ct)
    {
        await sender.Send(new DeleteCategoryCommand(id), ct);
        return Results.NoContent();
    }
}
