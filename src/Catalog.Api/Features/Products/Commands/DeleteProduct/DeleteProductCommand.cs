using MediatR;

namespace Catalog.Api.Features.Products.Commands.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest;
