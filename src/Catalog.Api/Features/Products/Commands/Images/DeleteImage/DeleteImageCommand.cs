using MediatR;

namespace Catalog.Api.Features.Products.Commands.Images.DeleteImage;

public record DeleteImageCommand(Guid VariantId, List<string> Urls) : IRequest<bool>;
