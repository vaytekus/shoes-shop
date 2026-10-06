using System.Data;
using FluentValidation;

namespace Catalog.Api.Features.Products.Commands.UpdateProduct;

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(c => c.Price)
            .GreaterThan(0);

        RuleFor(c => c.StockQuantity)
            .GreaterThanOrEqualTo(0);
    }
}
