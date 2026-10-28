using Catalog.Api.DTOs.SearchProducts;
using MediatR;

namespace Catalog.Api.Features.Products.Queries.SearchProducts;

public record SearchProductsQuery(
    string? Q,
    string? Brand,
    string? Season,
    string? Size,
    string? Section,
    string? Purpose,
    string? Category,
    int Page,
    int PageSize) : IRequest<SearchProductResult>;
