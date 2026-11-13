using Order.Api.Domain;

namespace Order.Api.DTOs;

public record CreateOrderRequest(
    string FirstName,
    string LastName,
    string Phone,
    string Email,
    DeliveryType DeliveryType,
    PaymentMethod PaymentMethod,
    List<OrderItemDto> Items,
    ShippingAddressDto ShippingAddress);
