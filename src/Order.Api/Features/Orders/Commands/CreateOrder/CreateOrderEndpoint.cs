using MediatR;
using Order.Api.DTOs;

namespace Order.Api.Features.Orders.Commands.CreateOrder;

public static class CreateOrderEndpoint
{
    public static async Task<IResult> Handle(
        IMediator mediator,
        CreateOrderRequest request,
        CancellationToken ct)
    {
        var command = new CreateOrderCommand(
            request.CustomerId,
            request.Items,
            request.ShippingAddress);

        var orderId = await mediator.Send(command, ct);

        return Results.CreatedAtRoute("GetOrder", new
        {
            id = orderId
        }, new
        {
            id = orderId
        });
    }
}
