using MediatR;

namespace Catalog.Api.Features.Products.Commands.ReorderVariants;

public record ReorderVariantsCommand(Guid ProductId, List<Guid> VariantIds) : IRequest;
