using MediatR;

namespace Catalog.Api.Features.Products.Commands.DeleteVariant;

public record DeleteVariantCommand(Guid VariantId) : IRequest;
