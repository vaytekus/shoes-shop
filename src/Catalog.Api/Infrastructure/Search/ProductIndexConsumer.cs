using Elastic.Clients.Elasticsearch;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ShoesShop.Shared.Events;

namespace Catalog.Api.Infrastructure.Search;

public class ProductIndexConsumer(
    CatalogDbContext db,
    ElasticsearchClient es) :
    IConsumer<ProductCreatedEvent>,
    IConsumer<ProductUpdatedEvent>
{
    public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
    {
        await IndexProduct(context.Message.ProductId, context.CancellationToken);
    }

    public async Task Consume(ConsumeContext<ProductUpdatedEvent> context)
    {
        await IndexProduct(context.Message.ProductId, context.CancellationToken);
    }

    private async Task IndexProduct(Guid productId, CancellationToken ct)
    {
        var p = await db.Products
            .Include(p => p.Category)
            .Include(p => p.Variants)
                .ThenInclude(v => v.Images)
            .Include(p => p.Variants)
                .ThenInclude(v => v.Sizes)
            .FirstOrDefaultAsync(p => p.Id == productId, ct);

        if (p is null)
        {
            return;
        }

        var doc = ProductDocumentMapper.ToDocument(p);
        await es.IndexAsync(doc, i => i.Index("products").Id(doc.Id), ct);
    }
}
