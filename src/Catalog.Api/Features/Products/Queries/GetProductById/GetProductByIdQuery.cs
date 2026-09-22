using Catalog.Api.DTOs;
using MediatR;

namespace Catalog.Api.Features.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductResponse?>;

