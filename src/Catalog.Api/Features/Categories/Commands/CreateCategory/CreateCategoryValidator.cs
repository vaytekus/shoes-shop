using FluentValidation;

namespace Catalog.Api.Features.Categories.Commands;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}
