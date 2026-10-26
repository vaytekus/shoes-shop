using MediatR;

namespace Catalog.Api.Features.Products.Commands.ReorderImages;

public record ReorderImagesCommand(Guid ProductId, Guid VariantId, List<string> Urls): IRequest<bool>;
