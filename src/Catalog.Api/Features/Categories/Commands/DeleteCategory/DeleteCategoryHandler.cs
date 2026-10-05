using Catalog.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryHandler(CatalogDbContext db) : IRequestHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand request, CancellationToken ct)
    {
        var product = await db.Categories
            .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

        if (product is null)
        {
            return;
        }

        db.Remove(product);
        await db.SaveChangesAsync(ct);
    }
}
