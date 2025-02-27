#pragma warning disable CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.

using MediatR;
using FluentValidation;

namespace Awc.Services.Product.Product.API.Application.Behaviors
{
    internal sealed class ValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public async Task<TResponse?> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors)
                                            .Where(f => f != null)
                                            .Select(failure => new Error(
                                                failure.PropertyName,
                                                failure.ErrorMessage))
                                            .Distinct()
                                            .ToArray();

            return failures.Length != 0 ? CreateResult<TResponse>(failures) : await next();
        }

        private static TResult? CreateResult<TResult>(Error[] errors)
            where TResult : Result
        {
            System.Text.StringBuilder sb = new();
            errors.ToList().ForEach(err => sb.AppendLine(err.Message));

            return Result<int>.Failure<int>(new Error("FluentValidationBehavior.Handle", sb.ToString())) as TResult;
        }
    }
}