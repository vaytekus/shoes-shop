using Catalog.Api.Features.Products.Queries.GetProducts;

namespace Catalog.Api.Features.Products;

public static class ProductsModule
{
    public static IEndpointRouteBuilder MapProducts(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products");

        group.MapGet("/", GetProductsEndpoint.Handle);

        return app;
    }
}
