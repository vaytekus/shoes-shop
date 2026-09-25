using MediatR;

namespace Order.Api.Features.Orders.Queries.GetCustomerOrders;

public static class GetCustomerOrdersEndpoint
{
    public static async Task<IResult> Handle(
        IMediator mediator,
        string customerId,
        CancellationToken ct,
        int page = 1,
        int pageSize = 10)
    {
        var result = await mediator.Send(new GetCustomerOrdersQuery(customerId, page, pageSize), ct);

        return Results.Ok(result);
    }
}
