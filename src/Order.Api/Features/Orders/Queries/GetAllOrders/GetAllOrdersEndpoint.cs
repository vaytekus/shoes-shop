using MediatR;

namespace Order.Api.Features.Orders.Queries.GetAllOrders;

public static class GetAllOrdersEndpoint
{
    public static async Task<IResult> Handle(
        IMediator mediator,
        CancellationToken ct,
        int pageSize = 10,
        int page = 1)
    {
        var result = await mediator.Send(new GetAllOrdersQuery(page, pageSize), ct);
        return Results.Ok(result);
    }
}
