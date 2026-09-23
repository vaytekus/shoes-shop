using FluentValidation;

namespace Basket.Api.Features.Basket.Commands.ClearBasket;

public class ClearBasketValidator : AbstractValidator<ClearBasketCommand>
{
    public ClearBasketValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
    }
}
