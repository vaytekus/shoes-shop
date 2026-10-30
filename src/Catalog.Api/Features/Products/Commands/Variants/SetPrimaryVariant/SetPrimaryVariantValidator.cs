using FluentValidation;

namespace Catalog.Api.Features.Products.Commands.Variants.SetPrimaryVariant;

public class SetPrimaryVariantValidator : AbstractValidator<SetPrimaryVariantCommand>
{
    public SetPrimaryVariantValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEqual(Guid.Empty);

        RuleFor(x => x.VariantId)
            .NotEqual(Guid.Empty);
    }
}
