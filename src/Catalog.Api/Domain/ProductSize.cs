namespace Catalog.Api.Domain;

public class ProductSize
{
    public Guid Id { get; set; }
    public Guid VariantId { get; set; }
    public ProductVariant Variant { get; set; }

    public string Size { get; set; } = null!;
    public int StockQuantity { get; set; }
}
