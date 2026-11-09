using FluentValidation;

namespace Basket.Api.Features.Basket.Commands.MergeBasket;

public class MergeBasketValidator : AbstractValidator<MergeBasketCommand>
{
    public MergeBasketValidator()
    {
        RuleFor(c => c.CustomerId)
            .NotEmpty();

        RuleFor(c => c.GuestId)
            .NotEmpty();
    }
}
