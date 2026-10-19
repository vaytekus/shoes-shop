using MediatR;

namespace Catalog.Api.Features.Products.Commands.CreateVariant;

public record CreateVariantCommand(
    Guid ProductId,
    string Name,
    string? Sku,
    string? ColorName,
    string? ColorHex) : IRequest<Guid>;
