using Order.Api.Domain;

namespace Order.Api.DTOs;

public record OrderResponse(
    Guid Id,
    string CustomerId,
    OrderStatus Status,
    decimal TotalAmount,
    DateTime CreatedAt,
    ShippingAddressDto ShippingAddress,
    List<OrderItemDto> Items);