using FluentValidation;
using Order.Api.Domain;

namespace Order.Api.Features.Orders.Commands.CreateOrder;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(o => o.FirstName).NotEmpty();
        RuleFor(o => o.LastName).NotEmpty();
        RuleFor(o => o.Phone).NotEmpty();
        RuleFor(o => o.Email).NotEmpty().EmailAddress();

        RuleFor(o => o.Items).NotEmpty();
        RuleForEach(o => o.Items).ChildRules(item => {
            item.RuleFor(o => o.ProductId).NotEmpty();
            item.RuleFor(o => o.Price).GreaterThan(0);
            item.RuleFor(o => o.Quantity).GreaterThan(0);
        });

        When(o => o.DeliveryType != DeliveryType.Pickup, () =>
        {
            RuleFor(o => o.ShippingAddress).NotNull();
            RuleFor(o => o.ShippingAddress.Street).NotEmpty();
            RuleFor(o => o.ShippingAddress.City).NotEmpty();
            RuleFor(o => o.ShippingAddress.Country).NotEmpty();
        });
    }
}
