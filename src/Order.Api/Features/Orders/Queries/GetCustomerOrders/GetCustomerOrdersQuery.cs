using MediatR;
using Order.Api.Domain;
using Order.Api.DTOs;
using ShoesShop.Shared.Pagination;

namespace Order.Api.Features.Orders.Queries.GetCustomerOrders;

public record GetCustomerOrdersQuery(
    string CustomerId,
    int Page = 1,
    int PageSize = 10
) : IRequest<PagedResult<OrderResponse>>;
