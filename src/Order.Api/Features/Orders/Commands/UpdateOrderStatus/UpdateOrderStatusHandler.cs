using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Api.Infrastructure;

namespace Order.Api.Features.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusHandler(OrderDbContext db) : IRequestHandler<UpdateOrderStatusCommand>
{
    public async Task Handle(UpdateOrderStatusCommand request, CancellationToken ct)
    {
        var order = await db.Orders.FirstOrDefaultAsync(x => x.Id == request.OrderId, ct)
            ?? throw new KeyNotFoundException($"Order {request.OrderId} not found");

        order.Status = request.Status;
        order.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);
    }
}
