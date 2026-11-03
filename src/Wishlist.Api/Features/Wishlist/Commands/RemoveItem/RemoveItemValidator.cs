using FluentValidation;
using Wishlist.Api.Infrastructure;

namespace Wishlist.Api.Features.Wishlist.Commands.RemoveItem;

public class RemoveItemValidator : AbstractValidator<RemoveItemCommand>
{
    public RemoveItemValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty();
        RuleFor(x => x.ProductId)
            .NotEmpty();
    }
}
