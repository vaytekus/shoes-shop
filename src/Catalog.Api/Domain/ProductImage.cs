namespace Catalog.Api.Domain;

public class ProductImage
{
    public Guid Id { get; set; }
    public Guid VariantId { get; set; }
    public ProductVariant Variant { get; set; } = null!;
    public string Url { get; set; } = null!;
    public int SortOrder { get; set; }
}
