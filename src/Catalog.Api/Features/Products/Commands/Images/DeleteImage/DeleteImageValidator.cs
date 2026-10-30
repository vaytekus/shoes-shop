using FluentValidation;

namespace Catalog.Api.Features.Products.Commands.Images.DeleteImage;

public class DeleteImageValidator : AbstractValidator<DeleteImageCommand>
{
    public DeleteImageValidator()
    {
        RuleFor(x => x.Urls)
            .NotEmpty()
            .WithMessage("Urls can not be empty");

        RuleForEach(x => x.Urls)
            .NotEmpty()
            .WithMessage("Urls can not be empty")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("Url must be a valid absolute url");
    }
}
