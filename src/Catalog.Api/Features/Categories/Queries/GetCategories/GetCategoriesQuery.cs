using Catalog.Api.DTOs;
using MediatR;

namespace Catalog.Api.Features.Categories.Queries.GetCategories;

public record GetCategoriesQuery() : IRequest<List<CategoryResponse>>;
