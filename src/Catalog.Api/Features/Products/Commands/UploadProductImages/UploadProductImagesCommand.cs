using MediatR;

namespace Catalog.Api.Features.Products.Commands.UploadProductImages;

public record UploadProductImagesCommand(Guid VariantId, IEnumerable<string> Urls) : IRequest<bool>;
