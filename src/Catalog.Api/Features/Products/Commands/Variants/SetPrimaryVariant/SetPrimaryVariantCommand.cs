using MediatR;

namespace Catalog.Api.Features.Products.Commands.Variants.SetPrimaryVariant;

public record SetPrimaryVariantCommand(Guid ProductId, Guid VariantId) : IRequest<bool>;
