using Catalog.Api.DTOs.SearchProducts;
using Catalog.Api.Infrastructure.Search;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Aggregations;
using Elastic.Clients.Elasticsearch.QueryDsl;
using MediatR;

namespace Catalog.Api.Features.Products.Queries.SearchProducts;

public class SearchProductsHandler(ElasticsearchClient es, ILogger<SearchProductsHandler> logger) : IRequestHandler<SearchProductsQuery, SearchProductResult>
{
    public async Task<SearchProductResult> Handle(SearchProductsQuery request, CancellationToken ct)
    {
        var response = await es.SearchAsync<ProductDocument>( s => s
            .Indices("products")
            .From((request.Page - 1) * request.PageSize)
            .Size(request.PageSize)
            .Query(q => q.Bool(b =>
            {
                var filters = new List<Query>();

                if (!string.IsNullOrEmpty(request.Q))
                {
                    b.Must(m => m.Bool(bb => bb
                        .Should(
                            s => s.MultiMatch(mm => mm
                                .Query(request.Q)
                                .Fields(new Field[]
                                {
                                    "name", "brand", "categoryName"
                                })),
                            s => s.MultiMatch(mm => mm
                                .Query(request.Q)
                                .Fields(new Field[]
                                {
                                    "name", "brand", "categoryName"
                                })
                            .Type(Elastic.Clients.Elasticsearch.QueryDsl.TextQueryType.PhrasePrefix))
                    )
                    .MinimumShouldMatch(1)));
                }

                AddFilter(filters, "brand.keyword",        request.Brand);
                AddFilter(filters, "season.keyword",       request.Season);
                AddFilter(filters, "sizes.keyword",        request.Size);
                AddFilter(filters, "section.keyword",      request.Section);
                AddFilter(filters, "purpose.keyword",      request.Purpose);
                AddFilter(filters, "categoryName.keyword", request.Category);

                if (filters.Count > 0) b.Filter(filters.ToArray());
            }))
            .Aggregations(new Dictionary<string, Aggregation>
            {
                ["brands"] = new TermsAggregation { Field = "brand.keyword",   Size = 50 },
                ["seasons"] = new TermsAggregation { Field = "season.keyword",  Size = 20 },
                ["sizes"] = new TermsAggregation { Field = "sizes.keyword",   Size = 30 },
                ["colors"] = new TermsAggregation { Field = "colors.keyword",  Size = 50 },
                ["purposes"] = new TermsAggregation { Field = "purpose.keyword", Size = 20 },
                ["categories"] = new TermsAggregation { Field = "categoryName.keyword", Size = 50 },
            }), ct);

        logger.LogInformation("ES valid={Valid} total={Total}",
            response.IsValidResponse,
            response.HitsMetadata?.Total?.Match(h => h?.Value ?? 0, l => l) ?? 0);

        var facets = new SearchFacets(
            Brands: GetBuckets(response, "brands"),
            Seasons: GetBuckets(response, "seasons"),
            Sizes: GetBuckets(response, "sizes"),
            Colors: GetBuckets(response, "colors"),
            Purposes: GetBuckets(response, "purposes"),
            Categories: GetBuckets(response, "categories"));

        var total = response.HitsMetadata?.Total?.Match(hits => hits?.Value ?? 0, l => l) ?? 0;
        return new SearchProductResult(response.Documents, total, facets);
    }

    private static void AddFilter(List<Query> filters, string field, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        var values = value.Split(',', StringSplitOptions.RemoveEmptyEntries);
        if(values.Length == 1)
        {
            filters.Add(new TermQuery { Field = field, Value = values[0] });
        }
        else
        {
            var should = values.Select(v => (Query)new TermQuery { Field = field, Value = v }).ToArray();
            filters.Add(new BoolQuery {Should =  should, MinimumShouldMatch = 1});
        }
    }

    private static IEnumerable<FacetItem> GetBuckets(SearchResponse<ProductDocument> response, string name) =>
        response.Aggregations?.GetStringTerms(name)?.Buckets
            .GroupBy(b => b.Key.Value?.ToString() ?? "")
            .Select(g => new FacetItem(g.Key, g.Sum(b => b.DocCount))) ?? [];

}
