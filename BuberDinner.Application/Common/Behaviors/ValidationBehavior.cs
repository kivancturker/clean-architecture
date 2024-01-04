using ErrorOr;
using FluentValidation;
using MediatR;

namespace BuberDinner.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : 
    IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
{
    private readonly IValidator<TRequest>? _validator;

    public ValidationBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (_validator is null)
        {
            return await next();
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        var errors = validationResult.Errors
            // ConvertAll select and convert to list
            .ConvertAll(validationFailure => Error.Validation(
                validationFailure.PropertyName,
                validationFailure.ErrorMessage));

        // Return dyanimc is not advisable
        return (dynamic) errors;
    }
}