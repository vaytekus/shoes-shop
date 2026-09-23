using FluentValidation;
using MediatR;

namespace Basket.Api.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (!validators.Any())
        {
            return await next(ct);
        }

        var context = new ValidationContext<TRequest>(request);

        var failure = validators
            .Select(v => v.Validate(context))
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failure.Count > 0)
        {
            throw new ValidationException(failure);
        }

        return await next(ct);
    }
}
