using FluentValidation;

namespace Order.Api.Features.Orders.Commands.SetTrackingNumber;

public class SetTrackingNumberValidator : AbstractValidator<SetTrackingNumberCommand>
{
    public SetTrackingNumberValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty();

        RuleFor(x => x.TrackingNumber)
            .NotEmpty();
    }
}
