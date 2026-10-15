using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryHandler(CatalogDbContext db) : IRequestHandler<UpdateCategoryCommand>
{
    public async Task Handle(UpdateCategoryCommand request, CancellationToken ct)
    {
        var category = await db.Categories.FirstOrDefaultAsync(c => c.Id == request.Id, ct);

        if (category is null)
        {
            throw new KeyNotFoundException($"Category with id {request.Id} not found");
        }

        category.Name = request.Name;
        await db.SaveChangesAsync(ct);
    }
}
