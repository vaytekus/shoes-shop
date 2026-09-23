using FluentValidation;

namespace Basket.Api.Features.Basket.Commands.RemoveItem;

public class RemoveItemValidator : AbstractValidator<RemoveItemCommand>
{
    public RemoveItemValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.CustomerId).NotEmpty();
    }
}
