using Order.Api.Domain;

namespace Order.Api.DTOs;

public record CreateOrderRequest(
    string CustomerId,
    List<OrderItemDto> Items,
    ShippingAddressDto ShippingAddress);
