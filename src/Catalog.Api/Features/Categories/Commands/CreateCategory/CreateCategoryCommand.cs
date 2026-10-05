using Catalog.Api.DTOs;
using MediatR;

namespace Catalog.Api.Features.Categories.Commands;

public record CreateCategoryCommand(string Name) : IRequest<CategoryResponse>;
