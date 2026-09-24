using Order.Api.Features.Orders.Commands.CreateOrder;
using Order.Api.Features.Orders.Queries.GetOrder;

namespace Order.Api.Features;

public static class OrdersModule
{
    public static IEndpointRouteBuilder MapOrders(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders");

        group.MapGet("/{id:guid}", GetOrderEndpoint.Handle)
            .WithName("GetOrder");

        group.MapPost("/", CreateOrderEndpoint.Handle)
            .WithName("CreateOrder");

        return app;
    }
}
