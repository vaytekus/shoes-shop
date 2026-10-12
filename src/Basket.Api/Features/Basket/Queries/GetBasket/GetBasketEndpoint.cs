using Basket.Api.Domain;
using MediatR;

namespace Basket.Api.Features.Basket.Queries.GetBasket;

public static class GetBasketEndpoint
{
    public static async Task<IResult> Handle(
        IMediator mediator,
        string customerId,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetBasketQuery(customerId), ct);
        return Results.Ok(result ?? new CustomerBasket { CustomerId = customerId });
    }
}
