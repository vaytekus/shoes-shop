using MediatR;
using Order.Api.DTOs;

namespace Order.Api.Features.Orders.Queries.GetOrder;

public record GetOrderQuery(Guid Id) : IRequest<OrderResponse?>;
