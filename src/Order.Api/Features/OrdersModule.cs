using Order.Api.Features.Orders.Commands.CreateOrder;
using Order.Api.Features.Orders.Commands.UpdateOrderStatus;
using Order.Api.Features.Orders.Queries.GetAllOrders;
using Order.Api.Features.Orders.Queries.GetCustomerOrders;
using Order.Api.Features.Orders.Queries.GetOrder;

namespace Order.Api.Features;

public static class OrdersModule
{
    public static IEndpointRouteBuilder MapOrders(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders").WithTags("Orders");

        group.MapGet("/", GetAllOrdersEndpoint.Handle)
            .WithName("GetAllOrders")
            .WithSummary("GetAllOrders");

        group.MapGet("/{id:guid}", GetOrderEndpoint.Handle)
            .WithName("GetOrder")
            .WithSummary("GetOrder");

        group.MapPost("/", CreateOrderEndpoint.Handle)
            .WithName("CreateOrder")
            .WithSummary("CreateOrder");

        group.MapGet("/customer/{customerId}", GetCustomerOrdersEndpoint.Handle)
            .WithName("GetCustomerOrders")
            .WithSummary("GetCustomerOrders");

        group.MapPut("{id:guid}/status", UpdateOrderStatusEndpoint.Handle)
            .WithName("UpdateOrderStatus")
            .WithSummary("UpdateOrderStatus");

        return app;
    }
}
