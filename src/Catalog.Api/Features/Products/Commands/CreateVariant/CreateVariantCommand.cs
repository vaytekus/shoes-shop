using MediatR;

namespace Catalog.Api.Features.Products.Commands.CreateVariant;

public record CreateVariantCommand(
    Guid ProductId,
    string ColorName,
    string? ColorHex) : IRequest<Guid>;
