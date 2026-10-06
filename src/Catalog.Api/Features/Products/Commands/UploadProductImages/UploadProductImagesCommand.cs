using MediatR;

namespace Catalog.Api.Features.Products.Commands.UploadProductImages;

public record UploadProductImagesCommand(Guid ProductId, IEnumerable<string> Urls) : IRequest<bool>;
