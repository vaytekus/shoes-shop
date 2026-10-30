using MediatR;

namespace Catalog.Api.Features.Products.Commands.Variants.UpdateVariant;

public record UpdateVariantCommand(
    Guid VariantId,
    string Name,
    string? Sku,
    string? ColorName,
    string? ColorHex) : IRequest;
