using MediatR;

namespace Catalog.Api.Features.Products.Commands.Variants.ReorderVariants;

public record ReorderVariantsCommand(Guid ProductId, List<Guid> VariantIds) : IRequest;
