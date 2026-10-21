namespace Catalog.Api.Domain;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Section { get; set; }
    public ICollection<Product> Products { get; set; } = [];
}
