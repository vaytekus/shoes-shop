using MediatR;
using Order.Api.Domain;
using Order.Api.DTOs;

namespace Order.Api.Features.Orders.Commands.CreateOrder;

public record CreateOrderCommand(
    string CustomerId,
    string FirstName,
    string LastName,
    string Phone,
    string Email,
    DeliveryType DeliveryType,
    PaymentMethod PaymentMethod,
    List<OrderItemDto> Items,
    ShippingAddressDto ShippingAddress) : IRequest<Guid>;
