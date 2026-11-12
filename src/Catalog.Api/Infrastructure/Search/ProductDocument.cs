namespace Catalog.Api.Infrastructure.Search;

public class ProductDocument
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;

    public string? Brand { get; set; }
    public string? Section { get; set; }
    public string? CategoryName { get; set; }

    public decimal? Price { get; set; }
    public int? DiscountPercent { get; set; }
    public bool IsNew { get; set; }
    public string? Season { get; set; }
    public string? Purpose { get; set; }
    public string? Style { get; set; }
    public string? Width { get; set; }

    public string? UpperMaterial { get; set; }
    public string? BrandCountry { get; set; }
    public string? ManufactureCountry { get; set; }

    public string? LiningMaterial { get; set; }
    public string? InsoleMaterial { get; set; }
    public string? SoleMaterial { get; set; }

    public List<string> Colors { get; set; } = [];
    public List<string> Sizes { get; set; } = [];
    public string? ImageUrl { get; set; }

    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
}
