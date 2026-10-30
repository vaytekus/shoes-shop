using MediatR;

namespace Catalog.Api.Features.Products.Commands.Images.ReorderImages;

public record ReorderImagesCommand(Guid ProductId, Guid VariantId, List<string> Urls): IRequest<bool>;
