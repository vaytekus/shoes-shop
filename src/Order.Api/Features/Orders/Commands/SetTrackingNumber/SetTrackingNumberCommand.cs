using MediatR;

namespace Order.Api.Features.Orders.Commands.SetTrackingNumber;

public record SetTrackingNumberCommand(Guid OrderId, string TrackingNumber) : IRequest;
