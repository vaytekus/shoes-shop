using Catalog.Api.Domain;
using Catalog.Api.DTOs;
using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Categories.Commands.CreateCategory;

public class CreateCategoryHandler(CatalogDbContext db): IRequestHandler<CreateCategoryCommand, CategoryResponse>
{
    public async Task<CategoryResponse> Handle(CreateCategoryCommand request, CancellationToken ct)
    {
        var exists = await db.Categories.AnyAsync(c => c.Name == request.Name, ct);
        if (exists)
        {
            throw new InvalidOperationException($"Category '{request.Name}' already exists");
        }

        var category = new Category{ Id = Guid.NewGuid(), Name = request.Name };
        db.Categories.Add(category);
        await db.SaveChangesAsync(ct);
        return new CategoryResponse(category.Id, category.Name);
    }
}
