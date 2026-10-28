using Catalog.Api.Infrastructure.Search;

namespace Catalog.Api.DTOs.SearchProducts;

public record SearchProductResult(
    IEnumerable<ProductDocument> Items,
    long Total,
    SearchFacets Facets);
