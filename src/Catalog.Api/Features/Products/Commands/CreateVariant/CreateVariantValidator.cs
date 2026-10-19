using FluentValidation;

namespace Catalog.Api.Features.Products.Commands.CreateVariant;

public class CreateVariantValidator : AbstractValidator<CreateVariantCommand>
{
    public CreateVariantValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(v => v.ColorHex)
            .Matches("^#([A-Fa-f0-9]{6})$")
            .When(v => v.ColorHex != null)
            .WithMessage("ColorHex must be a valid hex color, e.g. #FF5733");
    }
}
