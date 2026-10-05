using Catalog.Api.DTOs;
using MediatR;

namespace Catalog.Api.Features.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(string Name) : IRequest<CategoryResponse>;
