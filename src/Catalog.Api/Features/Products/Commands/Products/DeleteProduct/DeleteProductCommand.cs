using MediatR;

namespace Catalog.Api.Features.Products.Commands.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest<bool>;
