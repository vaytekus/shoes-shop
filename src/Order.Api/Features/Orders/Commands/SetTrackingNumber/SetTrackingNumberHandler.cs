using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Api.Infrastructure;

namespace Order.Api.Features.Orders.Commands.SetTrackingNumber;

public class SetTrackingNumberHandler(OrderDbContext db) : IRequestHandler<SetTrackingNumberCommand>
{
    public async Task Handle(SetTrackingNumberCommand request, CancellationToken ct)
    {
        var order = await db.Orders.FirstOrDefaultAsync(x => x.Id == request.OrderId, ct)
            ?? throw new KeyNotFoundException($"Order {request.OrderId} not found");

        order.TrackingNumber = request.TrackingNumber;
        order.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);
    }
}
