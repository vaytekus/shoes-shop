using MediatR;
using Order.Api.DTOs;

namespace Order.Api.Features.Orders.Commands.CreateOrder;

public record CreateOrderCommand(
    string CustomerId,
    List<OrderItemDto> Items,
    ShippingAddressDto ShippingAddress) : IRequest<Guid>;
