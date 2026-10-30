using MediatR;

namespace Catalog.Api.Features.Products.Commands.Variants.DeleteVariant;

public record DeleteVariantCommand(Guid VariantId) : IRequest<bool>;
