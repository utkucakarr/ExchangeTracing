using FluentValidation;
using MediatR;

namespace ExchangeTracing.BuildingBlocks.Behaviors;

/// <summary>
/// MediatR pipeline behavior that runs any FluentValidation validators registered for a
/// request before its handler. Registered once at the composition root so every module's
/// commands/queries are validated consistently.
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var failures = (await Task.WhenAll(
                    validators.Select(v => v.ValidateAsync(context, cancellationToken))))
                .SelectMany(result => result.Errors)
                .Where(failure => failure is not null)
                .ToList();

            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }
        }

        return await next();
    }
}
