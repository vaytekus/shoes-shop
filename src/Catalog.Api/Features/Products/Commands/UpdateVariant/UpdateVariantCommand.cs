using MediatR;

namespace Catalog.Api.Features.Products.Commands.UpdateVariant;

public record UpdateVariantCommand(
    Guid VariantId,
    string ColorName,
    string? ColorHex) : IRequest;
