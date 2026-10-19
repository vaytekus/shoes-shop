using Catalog.Api.DTOs;
using MediatR;
using ShoesShop.Shared.Pagination;

namespace Catalog.Api.Features.Products.Queries.GetProducts;

public record GetProductsQuery(
    int Page = 1,
    int PageSize = 10,
    Guid? CategoryId = null,
    string? Gender = null,
    bool? HasDiscount = null
) : IRequest<PagedResult<ProductResponse>>;
