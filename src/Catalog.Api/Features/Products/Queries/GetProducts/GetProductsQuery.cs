using Catalog.Api.DTOs;
using MediatR;

namespace Catalog.Api.Features.Products.Queries.GetProducts;

public record GetProductsQuery : IRequest<List<ProductResponse>>;
