namespace HCG.FondoRevolvente.Application.Common.Models;

public class Result
{
    internal Result(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }

    public bool Succeeded { get; }

    public string[] Errors { get; }

    public static Result Success()
    {
        return new Result(true, Array.Empty<string>());
    }

    public static Result Failure(IEnumerable<string> errors)
    {
        return new Result(false, errors);
    }
}

public class Result<T> : Result
{
    private Result(T value, bool succeeded, IEnumerable<string> errors) 
        : base(succeeded, errors)
    {
        Value = value;
    }

    public T Value { get; }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value, true, Array.Empty<string>());
    }

    public static new Result<T> Failure(IEnumerable<string> errors)
    {
        return new Result<T>(default!, false, errors);
    }
}
