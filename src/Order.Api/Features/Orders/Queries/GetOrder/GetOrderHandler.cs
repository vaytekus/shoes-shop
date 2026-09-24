using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Api.DTOs;
using Order.Api.Infrastructure;

namespace Order.Api.Features.Orders.Queries.GetOrder;

public class GetOrderHandler(OrderDbContext db) : IRequestHandler<GetOrderQuery, OrderResponse?>
{
    public async Task<OrderResponse?> Handle(GetOrderQuery request, CancellationToken ct)
    {
        var order = await db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == request.Id, ct);

        if (order is null)
        {
            return null;
        }

        return new OrderResponse(
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
            )).ToList());
    }
}
