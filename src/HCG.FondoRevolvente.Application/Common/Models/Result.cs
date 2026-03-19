namespace HCG.FondoRevolvente.Application.Common.Models;

/// <summary>
/// Representa el resultado de una operación que puede ser exitosa o fallida.
/// Implementa el patrón Result para evitar excepciones en el flujo normal.
/// </summary>
/// <typeparam name="T">Tipo del valor de retorno en caso de éxito.</typeparam>
public class Result<T>
{
    /// <summary>
    /// Indica si la operación fue exitosa.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Valor de retorno en caso de éxito.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Mensaje de error en caso de fallo.
    /// </summary>
    public string? Error { get; }

    /// <summary>
    /// Código de error para identificación programática.
    /// </summary>
    public string? ErrorCode { get; }

    /// <summary>
    /// Lista de errores de validación cuando aplica.
    /// </summary>
    public IReadOnlyList<string> ValidationErrors { get; }

    private Result(bool isSuccess, T? value, string? error, string? errorCode, 
        IReadOnlyList<string>? validationErrors = null)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        ErrorCode = errorCode;
        ValidationErrors = validationErrors ?? Array.Empty<string>();
    }

    /// <summary>
    /// Crea un resultado exitoso con un valor.
    /// </summary>
    public static Result<T> Success(T value) => new(true, value, null, null);

    /// <summary>
    /// Crea un resultado exitoso sin valor de retorno.
    /// </summary>
    public static Result<T> Success() => new(true, default, null, null);

    /// <summary>
    /// Crea un resultado fallido con un mensaje de error.
    /// </summary>
    public static Result<T> Failure(string error, string? errorCode = null) 
        => new(false, default, error, errorCode);

    /// <summary>
    /// Crea un resultado fallido con errores de validación.
    /// </summary>
    public static Result<T> ValidationFailure(IReadOnlyList<string> validationErrors)
        => new(false, default, "Errores de validación", "VALIDATION_ERROR", validationErrors);

    /// <summary>
    /// Crea un resultado fallido con errores de validación desde un diccionario.
    /// </summary>
    public static Result<T> ValidationFailure(IDictionary<string, string[]> errors)
    {
        var validationErrors = errors
            .SelectMany(kvp => kvp.Value.Select(v => $"{kvp.Key}: {v}"))
            .ToList();
        return ValidationFailure(validationErrors);
    }

    /// <summary>
    /// Crea un resultado fallido a partir de una excepción.
    /// </summary>
    public static Result<T> FromException(Exception exception, string? errorCode = null)
        => Failure(exception.Message, errorCode ?? exception.GetType().Name);

    /// <summary>
    /// Ejecuta una acción si el resultado es exitoso.
    /// </summary>
    public Result<T> OnSuccess(Action<T> action)
    {
        if (IsSuccess && Value is not null) action(Value);
        return this;
    }

    /// <summary>
    /// Ejecuta una función si el resultado es exitoso, transformando el valor.
    /// </summary>
    public Result<TResult> Map<TResult>(Func<T, TResult> func)
    {
        if (IsSuccess && Value is not null) return Result<TResult>.Success(func(Value));
        return Result<TResult>.Failure(Error!, ErrorCode);
    }

    /// <summary>
    /// Ejecuta una acción si el resultado es fallido.
    /// </summary>
    public Result<T> OnFailure(Action<string> action)
    {
        if (!IsSuccess) action(Error!);
        return this;
    }

    /// <summary>
    /// Obtiene el valor o devuelve un valor por defecto.
    /// </summary>
    public T GetValueOrDefault(T defaultValue) => IsSuccess ? Value! : defaultValue;

    /// <summary>
    /// Obtiene el valor o lanza una excepción si es fallido.
    /// </summary>
    public T GetValueOrThrow()
    {
        if (!IsSuccess) throw new InvalidOperationException(Error ?? "La operación falló");
        return Value!;
    }

    /// <summary>
    /// Conversión implícita desde un valor.
    /// </summary>
    public static implicit operator Result<T>(T value) => Success(value);
}

/// <summary>
/// Resultado sin valor de retorno para operaciones que no devuelven datos.
/// </summary>
public class Result
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public string? ErrorCode { get; }
    public IReadOnlyList<string> ValidationErrors { get; }

    private Result(bool isSuccess, string? error, string? errorCode, 
        IReadOnlyList<string>? validationErrors = null)
    {
        IsSuccess = isSuccess;
        Error = error;
        ErrorCode = errorCode;
        ValidationErrors = validationErrors ?? Array.Empty<string>();
    }

    public static Result Success() => new(true, null, null);
    public static Result Failure(string error, string? errorCode = null) 
        => new(false, error, errorCode);
    public static Result ValidationFailure(IReadOnlyList<string> validationErrors)
        => new(false, "Errores de validación", "VALIDATION_ERROR", validationErrors);
    public static Result<T> Success<T>(T value) => Result<T>.Success(value);
}
