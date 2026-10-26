using MediatR;

namespace Catalog.Api.Features.Products.Commands.ReorderVariants;

public static class ReorderVariantsEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        [Microsoft.AspNetCore.Mvc.FromBody] List<Guid> variantIds,
        IMediator mediator,
        CancellationToken ct)
    {
        await mediator.Send(new ReorderVariantsCommand(id, variantIds), ct);
        return Results.NoContent();
    }
}
