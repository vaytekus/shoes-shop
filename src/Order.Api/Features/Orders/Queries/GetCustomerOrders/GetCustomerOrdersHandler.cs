using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Api.Domain;
using Order.Api.DTOs;
using Order.Api.Infrastructure;
using ShoesShop.Shared.Pagination;

namespace Order.Api.Features.Orders.Queries.GetCustomerOrders;

public class GetCustomerOrdersHandler(OrderDbContext db) : IRequestHandler<GetCustomerOrdersQuery, PagedResult<OrderResponse>>
{
    public async Task<PagedResult<OrderResponse>> Handle(GetCustomerOrdersQuery request, CancellationToken ct)
    {
        var totalCount = await db.Orders
            .CountAsync(o => o.CustomerId == request.CustomerId, ct);

        var orders = await db.Orders
            .Include(o => o.Items)
            .Where(o => o.CustomerId == request.CustomerId)
            .OrderByDescending(o => o.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        var items = orders.Select(order => new OrderResponse(
        order.Id,
        order.CustomerId,
        order.Status,
        order.TotalAmount,
        order.CreatedAt,
        new ShippingAddressDto(
        order.ShippingAddress.Street,
        order.ShippingAddress.City,
        order.ShippingAddress.Country,
        order.ShippingAddress.ZipCode
        ),
        order.Items.Select(i => new OrderItemDto(
        i.ProductId,
        i.ProductName,
        i.Price,
        i.Quantity,
        i.ImageUrl
        )).ToList()
        )).ToList();

        return new PagedResult<OrderResponse>(items, totalCount, request.Page, request.PageSize);
    }
}
