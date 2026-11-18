using Catalog.Api.Domain;
using Catalog.Api.DTOs.StockCheck;
using Catalog.Api.Features.Products.Queries.CheckStock;
using Catalog.Api.Infrastructure;
using Catalog.Api.Tests.Helpers;

namespace Catalog.Api.Tests.Features.Products;

public class CheckStockHandlerTests
{
    private static async Task<(Product product, ProductVariant variant)> SeedAsync(
        CatalogDbContext db,
        string size,
        int stock)
    {
        var product = new Product { Id = Guid.NewGuid(), Name = "Test", Description = "d", Price = 100 };
        var variant = new ProductVariant { Id = Guid.NewGuid(), ProductId = product.Id, Name = "v" };
        var productSize = new ProductSize { Id = Guid.NewGuid(), VariantId = variant.Id, Size = size, StockQuantity = stock };

        db.Products.Add(product);
        db.ProductVariants.Add(variant);
        db.ProductSizes.Add(productSize);
        await db.SaveChangesAsync();

        return (product, variant);
    }

    [Fact]
    public async Task Handle_AllItemsInStock_ReturnsIsAvailableTrue()
    {
        var db = DbContextFactory.Create();
        var (product, _) = await SeedAsync(db, size: "42", stock: 5);
        var handler = new CheckStockHandler(db);
        var query = new CheckStockQuery([new StockCheckItem(product.Id, "42", 3)]);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.True(result.IsAvailable);
        Assert.Empty(result.UnavailableItems);
    }

    [Fact]
    public async Task Handle_OneItemOutOfStock_ReturnsIsAvailableFalseWithDetails()
    {
        var db = DbContextFactory.Create();
        var (product, _) = await SeedAsync(db, size: "42", stock: 2);
        var handler = new CheckStockHandler(db);
        var query = new CheckStockQuery([new StockCheckItem(product.Id, "42", 5)]);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.False(result.IsAvailable);
        Assert.Single(result.UnavailableItems);
        Assert.Equal(product.Id, result.UnavailableItems[0].ProductId);
        Assert.Equal("42", result.UnavailableItems[0].Size);
        Assert.Equal(2, result.UnavailableItems[0].Available);
        Assert.Equal(5, result.UnavailableItems[0].Requested);
    }

    [Fact]
    public async Task Handle_SizeNotFound_TreatsStockAsZeroAndReturnsUnavailable()
    {
        var db = DbContextFactory.Create();
        var (product, _) = await SeedAsync(db, size: "42", stock: 10);
        var handler = new CheckStockHandler(db);
        var query = new CheckStockQuery([new StockCheckItem(product.Id, "43", 1)]);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.False(result.IsAvailable);
        Assert.Single(result.UnavailableItems);
        Assert.Equal(0, result.UnavailableItems[0].Available);
    }

    [Fact]
    public async Task Handle_MultipleItems_ReturnsOnlyOutOfStockOnes()
    {
        var db = DbContextFactory.Create();
        var (product1, _) = await SeedAsync(db, size: "40", stock: 10);
        var (product2, _) = await SeedAsync(db, size: "41", stock: 1);
        var handler = new CheckStockHandler(db);
        var query = new CheckStockQuery([
            new StockCheckItem(product1.Id, "40", 2),
            new StockCheckItem(product2.Id, "41", 5),
        ]);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.False(result.IsAvailable);
        Assert.Single(result.UnavailableItems);
        Assert.Equal(product2.Id, result.UnavailableItems[0].ProductId);
    }
}
