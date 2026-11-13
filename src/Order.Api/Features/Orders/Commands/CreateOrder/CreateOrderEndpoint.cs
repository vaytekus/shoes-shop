using System.Security.Claims;
using MediatR;
using Order.Api.DTOs;

namespace Order.Api.Features.Orders.Commands.CreateOrder;

public static class CreateOrderEndpoint
{
    public static async Task<IResult> Handle(
        IMediator mediator,
        CreateOrderRequest request,
        ClaimsPrincipal user,
        CancellationToken ct)
    {
        var customerId = user.FindFirst("sub")?.Value ?? string.Empty;

        var command = new CreateOrderCommand(
            customerId,
            request.FirstName,
            request.LastName,
            request.Phone,
            request.Email,
            request.DeliveryType,
            request.PaymentMethod,
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
