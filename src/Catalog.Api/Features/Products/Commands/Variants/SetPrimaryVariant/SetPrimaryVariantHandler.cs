using Catalog.Api.Infrastructure;
using Catalog.Api.Infrastructure.Search;
using Elastic.Clients.Elasticsearch;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Features.Products.Commands.Variants.SetPrimaryVariant;

public class SetPrimaryVariantHandler(CatalogDbContext db, ElasticsearchClient es) : IRequestHandler<SetPrimaryVariantCommand, bool>
{
    public async Task<bool> Handle(SetPrimaryVariantCommand request, CancellationToken ct)
    {
        var variants = await db.ProductVariants
            .Where(v => v.ProductId == request.ProductId)
            .ToListAsync(ct);

        var target = variants
            .FirstOrDefault(v => v.Id == request.VariantId);

        if (target is null)
        {
            return false;
        }

        foreach (var v in variants)
        {
            v.IsPrimary = v.Id == request.VariantId;
        }

        var ordered = variants.OrderBy(v => v.SortOrder).ToList();
        ordered.Remove(target);
        ordered.Insert(0, target);

        for (var i = 0; i < ordered.Count; i++)
        {
            ordered[i].SortOrder = i;
        }

        await db.SaveChangesAsync(ct);

        var product = await db.Products
            .Include(p => p.Category)
            .Include(p => p.Variants).ThenInclude(v => v.Sizes)
            .Include(p => p.Variants).ThenInclude(v => v.Images)
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, ct);

        if (product is not null)
        {
            var doc = ProductDocumentMapper.ToDocument(product);
            await es.IndexAsync(doc, i => i.Index("products").Id(doc.Id), ct);
        }
        return true;
    }
}
