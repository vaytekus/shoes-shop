using FluentValidation;

namespace Catalog.Api.Features.Products.Commands.AddSize;

public class AddSizeValidator : AbstractValidator<AddSizeCommand>
{
    public AddSizeValidator()
    {
        RuleFor(s => s.Size)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(s => s.StockQuantity)
            .GreaterThanOrEqualTo(0);
    }
}
