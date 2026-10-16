using FluentValidation;

namespace Catalog.Api.Features.Products.Commands.CreateProduct;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(c => c.Price)
            .GreaterThan(0);

        RuleFor(c => c.DiscountPercent)
            .InclusiveBetween(0, 100)
            .When(c => c.DiscountPercent.HasValue);
    }
}
