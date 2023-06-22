namespace Functional;

public readonly struct Result<TOk, TError>
{
    public TOk Ok { get; }

    public TError Error { get; }

    public ResultStatus Status { get; }

    private Result(TOk? value, TError? error, ResultStatus status)
    {
        Ok = value!;
        Error = error!;
        Status = status;
    }

    public bool IsOk => Status == ResultStatus.Ok;

    public bool IsError => Status == ResultStatus.Error;

    public static implicit operator Result<TOk, TError>(Error<TError> error)
    {
        return new Result<TOk, TError>(default, error.Value, ResultStatus.Error);
    }

    public static implicit operator Result<TOk, TError>(Ok<TOk> ok)
    {
        return new Result<TOk, TError>(ok.Value, default, ResultStatus.Ok);
    }
}