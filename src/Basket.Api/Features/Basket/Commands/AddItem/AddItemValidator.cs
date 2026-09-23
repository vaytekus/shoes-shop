using FluentValidation;

namespace Basket.Api.Features.Basket.Commands.AddItem;

public class AddItemValidator : AbstractValidator<AddItemCommand>
{
    public AddItemValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.ProductName).NotEmpty();
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.ImageUrl).NotEmpty();
    }
}
