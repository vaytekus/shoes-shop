using MediatR;
using Order.Api.Domain;

namespace Order.Api.Features.Orders.Commands.UpdateOrderStatus;

public record UpdateOrderStatusCommand(
    Guid OrderId,
    OrderStatus Status) : IRequest;
