using MediatR;

namespace Order.Api.Features.Orders.Queries.GetOrder;

public static class GetOrderEndpoint
{
    public static async Task<IResult> Handle(
        IMediator mediator,
        Guid id,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetOrderQuery(id), ct);

        return result is null ? Results.NotFound() : Results.Ok(result);
    }
}
