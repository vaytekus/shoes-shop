using Catalog.Api.Domain;
using Catalog.Api.DTOs;
using Catalog.Api.Infrastructure;
using MediatR;

namespace Catalog.Api.Features.Categories.Commands;

public class CreateCategoryHandler(CatalogDbContext db): IRequestHandler<CreateCategoryCommand, CategoryResponse>
{
    public async Task<CategoryResponse> Handle(CreateCategoryCommand request, CancellationToken ct)
    {
        var category = new Category{ Id = Guid.NewGuid(), Name = request.Name };
        db.Categories.Add(category);
        await db.SaveChangesAsync(ct);
        return new CategoryResponse(category.Id, category.Name);
    }
}
