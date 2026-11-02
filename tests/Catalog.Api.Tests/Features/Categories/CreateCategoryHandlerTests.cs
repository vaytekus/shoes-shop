using Catalog.Api.Features.Categories.Commands.CreateCategory;
using Catalog.Api.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Tests.Features.Categories;

public class CreateCategoryHandlerTests
{
    [Fact]
    public async Task Handle_NewCategory_ReturnsResponseAndSavesToDb()
    {
        var db = DbContextFactory.Create();
        var handler = new CreateCategoryHandler(db);

        var result = await handler.Handle(new CreateCategoryCommand("Кросівки", "men"), CancellationToken.None);

        Assert.Equal("Кросівки", result.Name);
        Assert.Equal("men", result.Section);
        Assert.Equal(1, await db.Categories.CountAsync());
    }

    [Fact]
    public async Task Handle_DuplicateName_ThrowsInvalidOperationException()
    {
        var db = DbContextFactory.Create();
        var handler = new CreateCategoryHandler(db);
        await handler.Handle(new CreateCategoryCommand("Кросівки", "men"), CancellationToken.None);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.Handle(new CreateCategoryCommand("Кросівки", "women"), CancellationToken.None));
    }
}
