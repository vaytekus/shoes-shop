using Catalog.Api.Features.Categories.Commands.CreateCategory;
using Catalog.Api.Features.Categories.Commands.DeleteCategory;
using Catalog.Api.Features.Categories.Queries.GetCategories;

namespace Catalog.Api.Features.Categories;

public static class CategoriesModule
{
    public static IEndpointRouteBuilder MapCategories(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/categories")
            .WithTags("Categories");

        group.MapGet("/", GetCategoriesEndpoint.Handle)
            .WithName("GetCategories")
            .WithSummary("GetCategories");

        group.MapPost("/", CreateCategoryEndpoints.Handle)
            .WithName("CreateCategory")
            .WithSummary("CreateCategory")
            .RequireAuthorization();

        group.MapDelete("/{id:guid}", DeleteCategoryEndpoints.Handle)
            .WithName("DeleteCategory")
            .WithSummary("DeleteCategory")
            .RequireAuthorization();

        return group;
    }
}
