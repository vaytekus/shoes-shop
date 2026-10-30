using Catalog.Api.Infrastructure;
using Catalog.Api.Infrastructure.Search;
using Elastic.Clients.Elasticsearch;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Products.Commands.ReindexProducts;

public class ReindexProductsHandler(
    CatalogDbContext db,
    ElasticsearchClient es,
    ILogger<ReindexProductsHandler> logger) : IRequestHandler<ReindexProductsCommand, int>
{
    public async Task<int> Handle(ReindexProductsCommand request, CancellationToken ct)
    {
        var products = await db.Products
            .Include(p => p.Category)
            .Include(p => p.Variants)
            .ThenInclude(v => v.Sizes)
            .Include(p => p.Variants)
            .ThenInclude(v => v.Images)
            .ToListAsync(ct);

        logger.LogInformation("Reindexing {Count} products", products.Count);

        foreach (var p in products)
        {
            var doc = ProductDocumentMapper.ToDocument(p);
            await es.IndexAsync(doc, i => i.Index("products").Id(doc.Id), ct);
        }

        return products.Count;
    }
}
