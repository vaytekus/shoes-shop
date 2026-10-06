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

        RuleFor(c => c.StockQuantity)
            .GreaterThanOrEqualTo(0);
    }
}
