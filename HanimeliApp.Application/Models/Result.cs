namespace HanimeliApp.Application.Models;

public class Result<T> : Result
{
    public T? Data { get; set; }
}

public class Result
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }

    public static Result<TData> AsSuccess<TData>(TData data)
    {
        return AsSuccess(data, null);
    }

    public static Result<TData> AsSuccess<TData>(TData data, string? msg)
    {
        return new Result<TData>
        {
            IsSuccess = true,
            Data = data,
            Message = msg
        };
    }

    public static Result AsSuccess(string? msg = null)
    {
        return new Result
        {
            IsSuccess = true,
            Message = msg
        };
    }

    public static Result AsFailure(ExceptionBase exp)
    {
        var message = $"[{exp.ErrorCode}]:{exp.DisplayMessage}";

        return AsFailure(message);
    }

    public static Result AsFailure(string? msg = null)
    {
        return new Result
        {
            IsSuccess = false,
            Message = msg
        };
    }

    public static Result As(bool isSuccess, string? msg = null)
    {
        return new Result
        {
            IsSuccess = isSuccess,
            Message = msg
        };
    }
}