using Catalog.Api.Domain;
using Catalog.Api.Features.Categories.Commands.DeleteCategory;
using Catalog.Api.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Tests.Features.Categories;

public class DeleteCategoryHandlerTests
{
    [Fact]
    public async Task Handle_ExistingCategory_ReturnsTrueAndRemovesFromDb()
    {
        var db = DbContextFactory.Create();
        var category = new Category { Id = Guid.NewGuid(), Name = "Кросівки" };
        db.Categories.Add(category);
        await db.SaveChangesAsync();

        var handler = new DeleteCategoryHandler(db);
        var result = await handler.Handle(new DeleteCategoryCommand(category.Id), CancellationToken.None);

        Assert.True(result);
        Assert.Equal(0, await db.Categories.CountAsync());
    }

    [Fact]
    public async Task Handle_NonExistentCategory_ReturnsFalse()
    {
        var db = DbContextFactory.Create();
        var handler = new DeleteCategoryHandler(db);

        var result = await handler.Handle(new DeleteCategoryCommand(Guid.NewGuid()), CancellationToken.None);

        Assert.False(result);
    }
}
