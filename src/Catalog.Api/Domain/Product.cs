namespace Catalog.Api.Domain;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }

    // Base attributes
    public string? Brand { get; set; }
    public string? Sku { get; set; }
    public string Gender { get; set; }
    public int? DiscountPercent { get; set; }
    public bool IsNew { get; set; }

    //Materials
    public string? UpperMaterial { get; set; }
    public string? LiningMaterial { get; set; }
    public string? InsoleMaterial { get; set; }
    public string? SoleMaterial { get; set; }

    // Characteristics
    public string? Width { get; set; }
    public string? Season { get; set; }
    public string? HeelHeight { get; set; }
    public string? Purpose { get; set; }
    public string? Style { get; set; }

    // Production
    public string? BrandCountry { get; set; }
    public string? ManufactureCountry { get; set; }

    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Image collection
    public ICollection<ProductVariant> Variants { get; set; } = [];
}
