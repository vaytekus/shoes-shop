namespace Catalog.Api.Domain;

public class ProductVariant()
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    // product name
    public string Name { get; set; } = null!;
    public string? Sku { get; set; }

    // Color options
    public string ColorName { get; set; } = null!;
    public string? ColorHex { get; set; }

    // Photos and sizes
    public ICollection<ProductImage> Images { get; set; } = []!;
    public ICollection<ProductSize> Sizes { get; set; } = []!;
}
