using Catalog.Api.Domain;
using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Infrastructure.Search;

public class ElasticsearchReindexService(
        IServiceProvider services,
        ILogger<ElasticsearchReindexService> logger
    ) : IHostedService
{
    public async Task StartAsync(CancellationToken ct)
    {
        using var scope = services.CreateScope();
        var es = scope.ServiceProvider.GetRequiredService<ElasticsearchClient>();
        var db = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

        var count = await es.CountAsync<ProductDocument>(c => c.Indices("products"), ct);

        if (count.IsValidResponse && count.Count > 0)
        {
            logger.LogInformation("ES index 'products' has {Count} docs, skipping reindex", count.Count);
            return;
        }

        var products = await db.Products
            .Include(p => p.Category)
            .Include(p => p.Variants)
                .ThenInclude(v => v.Images)
            .Include(p => p.Variants)
                .ThenInclude(v => v.Sizes)
            .ToListAsync(ct);

        var ratingMap = await db.ProductReviews
            .Where(r => r.Status == ReviewStatus.Approved)
            .GroupBy(r => r.ProductId)
            .Select(g => new
            {
                ProductId = g.Key, Avg = g.Average(r => (double)r.Rating), Count = g.Count()
            })
            .ToDictionaryAsync(r => r.ProductId, ct);

        logger.LogInformation("Reindexing {Count} products into ES...", products.Count);

        foreach (var p in products)
        {
            var doc = ProductDocumentMapper.ToDocument(p);

            if (ratingMap.TryGetValue(p.Id, out var r))
            {
                doc.AverageRating = Math.Round(r.Avg, 1);
                doc.ReviewCount = r.Count;
            }

            await es.IndexAsync(doc, i => i.Index("products").Id(doc.Id), ct);
        }

        logger.LogInformation("Reindex complete");
    }

    public Task StopAsync(CancellationToken ct)
    {
        return Task.CompletedTask;
    }
}
