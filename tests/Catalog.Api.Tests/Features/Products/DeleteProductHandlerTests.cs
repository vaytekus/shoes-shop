using Catalog.Api.Domain;
using Catalog.Api.Features.Products.Commands.Products.DeleteProduct;
using Catalog.Api.Tests.Helpers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using ShoesShop.Shared.Events;

namespace Catalog.Api.Tests.Features.Products;

public class DeleteProductHandlerTests
{
    private static Product MakeProduct() => new()
    {
        Id = Guid.NewGuid(),
        Name = "Nike Air Max",
        Description = "Test",
        Price = 2500,
    };

    [Fact]
    public async Task Handle_ExistingProduct_ReturnsTruePublishesEventAndRemovesFromDb()
    {
        var db = DbContextFactory.Create();
        var publish = Substitute.For<IPublishEndpoint>();
        var product = MakeProduct();
        db.Products.Add(product);
        await db.SaveChangesAsync();

        var handler = new DeleteProductHandler(db, publish);
        var result = await handler.Handle(new DeleteProductCommand(product.Id), CancellationToken.None);

        Assert.True(result);
        Assert.Equal(0, await db.Products.CountAsync());
        await publish.Received(1).Publish(
            Arg.Is<ProductDeletedEvent>(e => e.ProductId == product.Id),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NonExistentProduct_ReturnsFalseAndDoesNotPublish()
    {
        var db = DbContextFactory.Create();
        var publish = Substitute.For<IPublishEndpoint>();
        var handler = new DeleteProductHandler(db, publish);

        var result = await handler.Handle(new DeleteProductCommand(Guid.NewGuid()), CancellationToken.None);

        Assert.False(result);
        await publish.DidNotReceive().Publish(Arg.Any<ProductDeletedEvent>(), Arg.Any<CancellationToken>());
    }
}
