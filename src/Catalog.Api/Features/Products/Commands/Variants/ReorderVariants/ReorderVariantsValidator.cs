using FluentValidation;

namespace Catalog.Api.Features.Products.Commands.Variants.ReorderVariants;

public class ReorderVariantsValidator : AbstractValidator<ReorderVariantsCommand>
{
    public ReorderVariantsValidator()
    {
        RuleFor(v => v.VariantIds)
            .NotEmpty()
            .WithMessage("Please specify at least one variant id");

        RuleForEach(v => v.VariantIds)
            .NotEqual(Guid.Empty)
            .WithMessage("Invalid variant id");
    }
}
