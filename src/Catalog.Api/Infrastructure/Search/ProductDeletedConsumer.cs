using Elastic.Clients.Elasticsearch;
using MassTransit;
using ShoesShop.Shared.Events;

namespace Catalog.Api.Infrastructure.Search;

public class ProductDeletedConsumer(ElasticsearchClient es) : IConsumer<ProductDeletedEvent>
{
    public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
    {
        await es.DeleteAsync<ProductDocument>(
            context.Message.ProductId.ToString(),
            d => d.Index("products"),
            context.CancellationToken
        );
    }
}
