using MediatR;

namespace Catalog.Api.Features.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(Guid Id, string Name, string? Section) : IRequest;
