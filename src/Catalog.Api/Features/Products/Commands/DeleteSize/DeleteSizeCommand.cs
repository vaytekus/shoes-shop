using MediatR;

namespace Catalog.Api.Features.Products.Commands.DeleteSize;

public record DeleteSizeCommand(Guid SizeId) : IRequest;
