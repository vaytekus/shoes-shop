using Catalog.Api.DTOs;
using MediatR;

namespace Catalog.Api.Features.Categories.Commands.UpdateCategory;

public static class UpdateCategoryEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        UpdateCategoryRequest request,
        IMediator mediator,
        CancellationToken ct)
    {
        await mediator.Send(new UpdateCategoryCommand(id, request.Name), ct);
        return Results.NoContent();
    }
}
