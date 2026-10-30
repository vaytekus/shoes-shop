using MediatR;

namespace Catalog.Api.Features.Products.Commands.Images.UploadProductImages;

public record UploadProductImagesCommand(Guid VariantId, IEnumerable<string> Urls) : IRequest<bool>;
