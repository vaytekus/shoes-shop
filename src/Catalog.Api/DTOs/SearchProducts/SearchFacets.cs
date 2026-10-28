namespace Catalog.Api.DTOs.SearchProducts;

public record SearchFacets(
    IEnumerable<FacetItem> Brands,
    IEnumerable<FacetItem> Seasons,
    IEnumerable<FacetItem> Sizes,
    IEnumerable<FacetItem> Colors,
    IEnumerable<FacetItem> Purposes,
    IEnumerable<FacetItem> Categories);
