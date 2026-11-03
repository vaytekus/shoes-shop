using FluentValidation;

namespace Wishlist.Api.Features.Wishlist.Commands.MergeItems;

public class MergeItemsValidator: AbstractValidator<MergeItemsCommand>
{
    public MergeItemsValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty();

        RuleFor(x => x.Items)
            .NotEmpty();

    }
}
