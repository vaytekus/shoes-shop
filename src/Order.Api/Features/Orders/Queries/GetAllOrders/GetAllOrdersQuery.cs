using MediatR;
using Order.Api.DTOs;
using ShoesShop.Shared.Pagination;

namespace Order.Api.Features.Orders.Queries.GetAllOrders;

public record GetAllOrdersQuery(
    int Page = 1,
    int PageSize = 10) : IRequest<PagedResult<OrderResponse>>;
