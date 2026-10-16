using Catalog.Api.Features.Products.Commands.AddSize;
using Catalog.Api.Features.Products.Commands.CreateProduct;
using Catalog.Api.Features.Products.Commands.CreateVariant;
using Catalog.Api.Features.Products.Commands.DeleteProduct;
using Catalog.Api.Features.Products.Commands.DeleteSize;
using Catalog.Api.Features.Products.Commands.DeleteVariant;
using Catalog.Api.Features.Products.Commands.UpdateProduct;
using Catalog.Api.Features.Products.Commands.UpdateSize;
using Catalog.Api.Features.Products.Commands.UpdateVariant;
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
            .RequireAuthorization("AdminOnly");

        group.MapGet("/{id:guid}", GetProductByIdEndpoint.Handle)
            .WithName("GetProductById")
            .WithSummary("GetProductById");

        group.MapPut("/{id:guid}", UpdateProductEndpoint.Handle)
            .WithName("UpdateProduct")
            .WithSummary("UpdateProduct")
            .RequireAuthorization("AdminOnly");

        group.MapDelete("/{id:guid}", DeleteProductEndpoint.Handle)
            .WithName("DeleteProduct")
            .WithSummary("DeleteProduct")
            .RequireAuthorization("AdminOnly");

        // Варіанти (колір)
        group.MapPost("/{id:guid}/variants", CreateVariantEndpoint.Handle)
            .WithName("CreateVariant")
            .WithSummary("CreateVariant")
            .RequireAuthorization("AdminOnly");

        group.MapPut("/{id:guid}/variants/{variantId:guid}", UpdateVariantEndpoint.Handle)
            .WithName("UpdateVariant")
            .WithSummary("UpdateVariant")
            .RequireAuthorization("AdminOnly");

        group.MapDelete("/{id:guid}/variants/{variantId:guid}", DeleteVariantEndpoint.Handle)
            .WithName("DeleteVariant")
            .WithSummary("DeleteVariant")
            .RequireAuthorization("AdminOnly");

        group.MapPost("/{id:guid}/variants/{variantId:guid}/images", UploadProductImagesEndpoint.Handle)
            .WithName("UploadVariantImages")
            .WithSummary("UploadVariantImages")
            .DisableAntiforgery()
            .RequireAuthorization("AdminOnly");

        // Розміри варіанту
        group.MapPost("/{id:guid}/variants/{variantId:guid}/sizes", AddSizeEndpoint.Handle)
            .WithName("AddSize")
            .WithSummary("AddSize")
            .RequireAuthorization("AdminOnly");

        group.MapPut("/{id:guid}/variants/{variantId:guid}/sizes/{sizeId:guid}", UpdateSizeEndpoint.Handle)
            .WithName("UpdateSize")
            .WithSummary("UpdateSize")
            .RequireAuthorization("AdminOnly");

        group.MapDelete("/{id:guid}/variants/{variantId:guid}/sizes/{sizeId:guid}", DeleteSizeEndpoint.Handle)
            .WithName("DeleteSize")
            .WithSummary("DeleteSize")
            .RequireAuthorization("AdminOnly");

        return app;
    }
}
