using Catalog.Api.Domain;

namespace Catalog.Api.Infrastructure.Search;

public static class ProductDocumentMapper
{
    public static ProductDocument ToDocument(Product p) => new()
    {
        Id = p.Id.ToString(),
        Name = p.Name,
        Brand = p.Brand,
        Section = p.Section,
        CategoryName = p.Category?.Name,
        Price = p.Price,
        DiscountPercent = p.DiscountPercent,
        IsNew = p.IsNew,
        Season = p.Season,
        Purpose = p.Purpose,
        Style = p.Style,
        Width = p.Width,
        UpperMaterial = p.UpperMaterial,
        BrandCountry = p.BrandCountry,
        ManufactureCountry = p.ManufactureCountry,
        LiningMaterial = p.LiningMaterial,
        InsoleMaterial = p.InsoleMaterial,
        SoleMaterial = p.SoleMaterial,
        Colors = p.Variants
            .Select(v => v.ColorName)
            .Where(c => c != null)
            .Select(c => c!)
            .Distinct()
            .ToList(),
        Sizes = p.Variants
            .SelectMany(v => v.Sizes)
            .Select(s => s.Size).Distinct().ToList(),
        ImageUrl = (p.Variants
            .FirstOrDefault(v => v.IsPrimary) ?? p.Variants.FirstOrDefault())?
            .Images.OrderBy(i => i.SortOrder)
            .FirstOrDefault()?.Url,
    };
}
