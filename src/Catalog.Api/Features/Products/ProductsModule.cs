using Catalog.Api.Features.Products.Commands.CreateProduct;
using Catalog.Api.Features.Products.Commands.DeleteProduct;
using Catalog.Api.Features.Products.Commands.UpdateProduct;
using Catalog.Api.Features.Products.Queries.GetProductById;
using Catalog.Api.Features.Products.Queries.GetProducts;

namespace Catalog.Api.Features.Products;

public static class ProductsModule
{
    public static IEndpointRouteBuilder MapProducts(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products");

        group.MapGet("/", GetProductsEndpoint.Handle)
            .WithName("GetProducts");

        group.MapPost("/", CreateProductEndpoint.Handle)
            .WithName("CreateProduct");

        group.MapGet("/{id:guid}", GetProductByIdEndpoint.Handle)
            .WithName("GetProductById");

        group.MapPut("/{id:guid}", UpdateProductEndpoint.Handle)
            .WithName("UpdateProduct");

        group.MapDelete("/{id:guid}", DeleteProductEndpoint.Handle)
            .WithName("DeleteProduct");

        return app;
    }
}
