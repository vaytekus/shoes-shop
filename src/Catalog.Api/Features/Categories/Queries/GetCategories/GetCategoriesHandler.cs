using Catalog.Api.DTOs;
using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Categories.Queries.GetCategories;

public class GetCategoriesHandler(CatalogDbContext db) : IRequestHandler<GetCategoriesQuery, List<CategoryResponse>>
{
    public async Task<List<CategoryResponse>> Handle(GetCategoriesQuery request, CancellationToken ct)
    {
        return await db.Categories
            .Select(c => new CategoryResponse(c.Id, c.Name))
            .ToListAsync(ct);
    }
}
