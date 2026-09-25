using MediatR;
using Order.Api.DTOs;

namespace Order.Api.Features.Orders.Commands.UpdateOrderStatus;

public static class UpdateOrderStatusEndpoint
{
    public static async Task<IResult> Handle(
        IMediator mediator,
        Guid id,
        UpdateOrderStatusRequest request,
        CancellationToken ct)
    {
        await mediator.Send(new UpdateOrderStatusCommand(id, request.Status), ct);
        return Results.NoContent();
    }
}
