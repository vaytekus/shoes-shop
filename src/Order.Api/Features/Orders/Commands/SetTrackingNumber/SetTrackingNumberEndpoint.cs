using MediatR;
using Order.Api.DTOs;

namespace Order.Api.Features.Orders.Commands.SetTrackingNumber;

public static class SetTrackingNumberEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        SetTrackingNumberRequest request,
        ISender sender,
        CancellationToken ct)
    {
        await sender.Send(new SetTrackingNumberCommand(id, request.TrackingNumber), ct);
        return Results.NoContent();
    }
}
