using FluentValidation;
using MediatR;
using HCG.FondoRevolvente.Application.Common.Exceptions;

namespace HCG.FondoRevolvente.Application.Common.Behaviors;

/// <summary>
/// Behavior de MediatR que valida automáticamente los requests.
/// </summary>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Omitir si no hay validadores
        if (!_validators.Any())
        {
            return await next();
        }

        // Omitir si el request marca ISkipValidation
        if (request is ISkipValidation)
        {
            return await next();
        }

        // Crear contexto y ejecutar validaciones en paralelo
        var context = new ValidationContext<TRequest>(request);
        
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // Recopilar errores
        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            // Agrupar errores por propiedad
            var errors = failures
                .GroupBy(f => f.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(f => f.ErrorMessage).ToArray());

            throw new HCG.FondoRevolvente.Application.Common.Exceptions.ValidationException(errors);
        }

        return await next();
    }
}

/// <summary>
/// Interfaz marcadora para requests que omiten validación automática.
/// </summary>
public interface ISkipValidation { }
