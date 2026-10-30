using FluentValidation;

namespace Catalog.Api.Features.Products.Commands.Images.ReorderImages;

public class ReorderImagesValidator : AbstractValidator<ReorderImagesCommand>
{
    public ReorderImagesValidator()
    {
        RuleFor(x => x.Urls)
            .NotEmpty()
            .WithMessage("Please specify at least one url");

        RuleForEach(x => x.Urls)
            .NotEmpty()
            .WithMessage("Please specify at least one url")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("Invalid url");
    }
}
