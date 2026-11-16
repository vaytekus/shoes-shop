using Microsoft.EntityFrameworkCore;
using Order.Api.Domain;

namespace Order.Api.Infrastructure;

public class OrderTrackingBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<OrderTrackingBackgroundService> logger) : BackgroundService
{
    private static readonly TimeSpan _interval = TimeSpan.FromMinutes(30);
    private const string _receivedStatusCode = "9";
    private const string _refusedStatusCode = "7";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_interval, stoppingToken);
            await CheckTrackingAsync(stoppingToken);
        }
    }

    private async Task CheckTrackingAsync(CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        var client = scope.ServiceProvider.GetRequiredService<NovaPoshtaTrackingClient>();

        var orders = await db.Orders
            .Where(o => o.Status == OrderStatus.Shipped && o.TrackingNumber != string.Empty)
            .ToListAsync(ct);

        foreach (var order in orders)
        {
            try
            {
                var statusCode = await client.GetStatusCodeAsync(order.TrackingNumber, ct);
                if (statusCode == _receivedStatusCode)
                {
                    order.Status = OrderStatus.Delivered;
                    order.UpdatedAt = DateTime.UtcNow;
                    logger.LogInformation("Order {OrderId} → Delivered via NP tracking", order.Id);
                }
                else if (statusCode == _refusedStatusCode)
                {
                    order.Status = OrderStatus.Returned;
                    order.UpdatedAt = DateTime.UtcNow;
                    logger.LogInformation("Order {OrderId} → Returned via NP tracking", order.Id);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "NP tracking failed for order {OrderId}", order.Id);
            }
        }

        await db.SaveChangesAsync(ct);
    }
}
