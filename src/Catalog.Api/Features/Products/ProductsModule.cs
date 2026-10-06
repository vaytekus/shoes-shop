using Catalog.Api.Features.Products.Commands.CreateProduct;
using Catalog.Api.Features.Products.Commands.DeleteProduct;
using Catalog.Api.Features.Products.Commands.UpdateProduct;
using Catalog.Api.Features.Products.Commands.UploadProductImages;
using Catalog.Api.Features.Products.Queries.GetProductById;
using Catalog.Api.Features.Products.Queries.GetProducts;

namespace Catalog.Api.Features.Products;

public static class ProductsModule
{
    public static IEndpointRouteBuilder MapProducts(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products")
            .WithTags("Products");

        group.MapGet("/", GetProductsEndpoint.Handle)
            .WithName("GetProducts")
            .WithSummary("GetProducts");

        group.MapPost("/", CreateProductEndpoint.Handle)
            .WithName("CreateProduct")
            .WithSummary("CreateProduct")
            .RequireAuthorization();

        group.MapGet("/{id:guid}", GetProductByIdEndpoint.Handle)
            .WithName("GetProductById")
            .WithSummary("GetProductById");

        group.MapPut("/{id:guid}", UpdateProductEndpoint.Handle)
            .WithName("UpdateProduct")
            .WithSummary("UpdateProduct")
            .RequireAuthorization();

        group.MapDelete("/{id:guid}", DeleteProductEndpoint.Handle)
            .WithName("DeleteProduct")
            .WithSummary("DeleteProduct")
            .RequireAuthorization();

        group.MapPost("/{id:guid}/images", UploadProductImagesEndpoint.Handle)
            .WithName("UploadProductImages")
            .WithSummary("UploadProductImages")
            .DisableAntiforgery()
            .RequireAuthorization();

        return app;
    }
}
