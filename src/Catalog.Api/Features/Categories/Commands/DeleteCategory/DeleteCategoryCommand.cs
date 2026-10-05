using Catalog.Api.DTOs;
using MediatR;

namespace Catalog.Api.Features.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : IRequest;
