using FluentValidation;

namespace Order.Api.Features.Orders.Commands.CreateOrder;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(o => o.CustomerId).NotEmpty();
        RuleFor(o => o.Items).NotEmpty();
        RuleForEach(o => o.Items).ChildRules(item => {
            item.RuleFor(o => o.ProductId).NotEmpty();
            item.RuleFor(o => o.Price).GreaterThan(0);
            item.RuleFor(o => o.Quantity).GreaterThan(0);
        });
        RuleFor(o => o.ShippingAddress).NotNull();
        RuleFor(o => o.ShippingAddress.Street).NotEmpty();
        RuleFor(o => o.ShippingAddress.City).NotEmpty();
        RuleFor(o => o.ShippingAddress.Country).NotEmpty();
        RuleFor(o => o.ShippingAddress.ZipCode).NotEmpty();
    }
}
