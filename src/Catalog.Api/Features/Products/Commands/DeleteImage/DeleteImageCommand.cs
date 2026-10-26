using MediatR;

namespace Catalog.Api.Features.Products.Commands.DeleteImage;

public record DeleteImageCommand(Guid VariantId, List<string> Urls) : IRequest;
