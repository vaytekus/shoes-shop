using Basket.Api.Features.Basket.Commands.AddItem;
using Basket.Api.Features.Basket.Commands.ClearBasket;
using Basket.Api.Features.Basket.Commands.RemoveItem;
using Basket.Api.Features.Basket.Commands.UpdateItem;
using Basket.Api.Features.Basket.Queries.GetBasket;

namespace Basket.Api.Features.Basket;

public static class BasketsModule
{
    public static IEndpointRouteBuilder MapBasket(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/basket");

        group.MapGet("/{customerId}", GetBasketEndpoint.Handle)
            .WithName("GetBasket");

        group.MapPost("/{customerId}/items", AddItemEndpoint.Handle)
            .WithName("AddProduct");

        group.MapDelete("/{customerId}/items/{productId}", RemoveItemEndpoint.Handle)
            .WithName("RemoveProduct");

        group.MapDelete("/{customerId}", ClearBasketEndpoint.Handle)
            .WithName("ClearBasket");

        group.MapPut("/{customerId}/items/{productId}", UpdateItemEndpoint.Handle)
            .WithName("UpdateProduct");

        return app;
    }
}
