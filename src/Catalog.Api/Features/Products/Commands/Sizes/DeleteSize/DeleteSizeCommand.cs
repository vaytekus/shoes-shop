using MediatR;

namespace Catalog.Api.Features.Products.Commands.Sizes.DeleteSize;

public record DeleteSizeCommand(Guid SizeId) : IRequest<bool>;
