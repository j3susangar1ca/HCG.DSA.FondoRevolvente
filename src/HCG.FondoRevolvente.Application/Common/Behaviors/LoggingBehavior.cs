using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HCG.FondoRevolvente.Application.Common.Behaviors;

/// <summary>
/// Behavior que registra información de las operaciones.
/// </summary>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private static readonly string[] SensitiveProperties = 
    {
        "Password", "PasswordConfirmation", "CurrentPassword", "NewPassword",
        "Token", "AccessToken", "RefreshToken", "Secret", "ApiKey",
        "Rfc", "Curp", "Clabe", "NumeroCuenta"
    };

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var stopwatch = Stopwatch.StartNew();

        // Logging detallado o simplificado según interfaz
        if (request is ISkipDetailedLogging)
        {
            _logger.LogDebug(
                "Iniciando: {RequestName}", requestName);
        }
        else
        {
            _logger.LogInformation(
                "Iniciando operación: {RequestName} - Datos: {@Request}",
                requestName,
                SanitizeRequest(request));
        }

        try
        {
            var response = await next();
            stopwatch.Stop();

            _logger.LogInformation(
                "Operación completada: {RequestName} - Duración: {ElapsedMilliseconds}ms",
                requestName,
                stopwatch.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(
                ex,
                "Error en operación: {RequestName} - Duración: {ElapsedMilliseconds}ms - " +
                "Error: {ErrorMessage}",
                requestName,
                stopwatch.ElapsedMilliseconds,
                ex.Message);

            throw;
        }
    }

    /// <summary>
    /// Sanitiza el request removiendo datos sensibles.
    /// </summary>
    private static object SanitizeRequest(TRequest request)
    {
        if (request == null) return "{}";

        var requestType = typeof(TRequest);
        var properties = requestType.GetProperties();
        var sanitized = new Dictionary<string, object?>();

        foreach (var prop in properties)
        {
            try
            {
                var propName = prop.Name;
                var value = prop.GetValue(request);

                if (IsSensitiveProperty(propName))
                {
                    sanitized[propName] = "***REDACTED***";
                }
                else if (value != null)
                {
                    var stringValue = value.ToString();
                    if (stringValue != null && stringValue.Length > 500)
                    {
                        sanitized[propName] = stringValue.Substring(0, 500) + "...[TRUNCATED]";
                    }
                    else
                    {
                        sanitized[propName] = value;
                    }
                }
                else
                {
                    sanitized[propName] = null;
                }
            }
            catch
            {
                // Si no se puede leer la propiedad, omitir
            }
        }

        return sanitized;
    }

    private static bool IsSensitiveProperty(string propertyName)
    {
        return SensitiveProperties.Any(sp => 
            propertyName.Contains(sp, StringComparison.OrdinalIgnoreCase));
    }
}

/// <summary>
/// Interfaz para requests que omiten logging detallado.
/// </summary>
public interface ISkipDetailedLogging { }
