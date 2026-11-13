using Order.Api.Domain;

namespace Order.Api.DTOs;

public record OrderResponse(
    Guid Id,
    string CustomerId,
    OrderStatus Status,
    string FirstName,
    string LastName,
    string Phone,
    string Email,
    DeliveryType DeliveryType,
    PaymentMethod PaymentMethod,
    decimal TotalAmount,
    DateTime CreatedAt,
    ShippingAddressDto ShippingAddress,
    List<OrderItemDto> Items);