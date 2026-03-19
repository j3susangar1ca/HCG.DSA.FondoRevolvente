namespace HCG.FondoRevolvente.Application.Common.Exceptions;

/// <summary>
/// Excepción base para errores de la capa de Application.
/// </summary>
public class ApplicationExceptionBase : Exception
{
    public string ErrorCode { get; }
    public IDictionary<string, object>? Details { get; }

    public ApplicationExceptionBase(string message, string errorCode, 
        IDictionary<string, object>? details = null)
        : base(message)
    {
        ErrorCode = errorCode;
        Details = details;
    }

    public ApplicationExceptionBase(string message, string errorCode, 
        Exception innerException, IDictionary<string, object>? details = null)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        Details = details;
    }
}

/// <summary>
/// Excepción lanzada cuando una entidad no es encontrada.
/// </summary>
public class NotFoundException : ApplicationExceptionBase
{
    public Type EntityType { get; }
    public object EntityId { get; }

    public NotFoundException(Type entityType, object entityId)
        : base($"No se encontró {entityType.Name} con ID '{entityId}'", "NOT_FOUND")
    {
        EntityType = entityType;
        EntityId = entityId;
    }

    public NotFoundException(string message)
        : base(message, "NOT_FOUND")
    {
        EntityType = typeof(object);
        EntityId = string.Empty;
    }

    public static NotFoundException For<T>(object entityId)
        => new(typeof(T), entityId);
}

/// <summary>
/// Excepción lanzada cuando falla la validación de datos.
/// </summary>
public class ValidationException : ApplicationExceptionBase
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(IDictionary<string, string[]> errors)
        : base("Errores de validación", "VALIDATION_ERROR", 
              errors.ToDictionary(e => e.Key, e => (object)e.Value))
    {
        Errors = errors;
    }

    public ValidationException(string field, string error)
        : this(new Dictionary<string, string[]> { [field] = new[] { error } })
    {
    }

    public ValidationException(IEnumerable<string> errors)
        : this(new Dictionary<string, string[]> { ["General"] = errors.ToArray() })
    {
    }
}

/// <summary>
/// Excepción lanzada cuando una operación no está permitida.
/// </summary>
public class ForbiddenAccessException : ApplicationExceptionBase
{
    public ForbiddenAccessException(string? message = null)
        : base(message ?? "No tiene permisos para realizar esta operación", "FORBIDDEN")
    {
    }

    public ForbiddenAccessException(string resource, string action)
        : base($"No tiene permisos para {action} en {resource}", "FORBIDDEN")
    {
    }
}

/// <summary>
/// Excepción lanzada cuando hay conflictos de concurrencia.
/// </summary>
public class ConcurrencyException : ApplicationExceptionBase
{
    public ConcurrencyException(string? message = null)
        : base(message ?? "El registro ha sido modificado por otro usuario", 
               "CONCURRENCY_ERROR")
    {
    }
}

/// <summary>
/// Excepción lanzada cuando una entidad ya existe.
/// </summary>
public class AlreadyExistsException : ApplicationExceptionBase
{
    public Type EntityType { get; }

    public AlreadyExistsException(Type entityType, string identifier)
        : base($"Ya existe {entityType.Name} con identificador '{identifier}'", 
               "ALREADY_EXISTS")
    {
        EntityType = entityType;
    }

    public static AlreadyExistsException For<T>(string identifier)
        => new(typeof(T), identifier);
}

/// <summary>
/// Excepción lanzada cuando una regla de negocio es violada.
/// </summary>
public class BusinessRuleException : ApplicationExceptionBase
{
    public string RuleCode { get; }

    public BusinessRuleException(string ruleCode, string message)
        : base(message, "BUSINESS_RULE_VIOLATION")
    {
        RuleCode = ruleCode;
    }

    public BusinessRuleException(string ruleCode, string message, 
        IDictionary<string, object> details)
        : base(message, "BUSINESS_RULE_VIOLATION", details)
    {
        RuleCode = ruleCode;
    }
}

/// <summary>
/// Excepción lanzada cuando un servicio externo falla.
/// </summary>
public class ExternalServiceException : ApplicationExceptionBase
{
    public string ServiceName { get; }

    public ExternalServiceException(string serviceName, string message, 
        Exception? innerException = null)
        : base(message, "EXTERNAL_SERVICE_ERROR", innerException)
    {
        ServiceName = serviceName;
    }

    public static ExternalServiceException SatService(string message, 
        Exception? innerException = null)
        => new("SAT", message, innerException);
}
