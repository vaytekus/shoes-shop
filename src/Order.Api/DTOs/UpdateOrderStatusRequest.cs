using Order.Api.Domain;

namespace Order.Api.DTOs;

public record UpdateOrderStatusRequest(OrderStatus Status);
