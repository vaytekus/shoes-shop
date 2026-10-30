using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryHandler(CatalogDbContext db) : IRequestHandler<DeleteCategoryCommand, bool>
{
    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken ct)
    {
        var category = await db.Categories
            .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

        if (category is null)
        {
            return false;
        }

        db.Remove(category);
        await db.SaveChangesAsync(ct);
        return true;
    }
}
