using Catalog.Api.DTOs;
using MediatR;
using ShoesShop.Shared.Pagination;

namespace Catalog.Api.Features.Products.Queries.GetProducts;

public record GetProductsQuery(
    int Page = 1,
    int PageSize = 10
) : IRequest<PagedResult<ProductResponse>>;
