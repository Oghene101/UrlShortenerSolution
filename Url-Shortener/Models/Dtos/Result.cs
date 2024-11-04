using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace Url_Shortener.Models.Dtos;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IEnumerable<Error>? Errors { get; }

    public Result(bool isSuccess, IEnumerable<Error>? errors)
    {
        if (isSuccess && !errors.IsNullOrEmpty())
            throw new InvalidOperationException("Cannot be successful with errors.");

        if (!isSuccess && errors.IsNullOrEmpty())
            throw new InvalidOperationException("Cannot be unsuccessful without errors.");

        IsSuccess = isSuccess;
        Errors = errors;
    }

    public static Result Success()
    {
        return new Result(true, Error.None);
    }

    public static Result Failure(IEnumerable<Error> errors)
    {
        return new Result(false, errors);
    }

    public static implicit operator Result(Error[] errors)
    {
        return Failure(errors);
    }
}

public class Result<T>(bool isSuccess, T data, IEnumerable<Error> errors) : Result(isSuccess, errors)
{
    public T Data { get; } = data ?? throw new ArgumentException("Data cannot be null.", nameof(data));

    public static Result<T> Success(T data)
    {
        return new Result<T>(true, data, Error.None);
    }

    public static implicit operator Result<T>(T data)
    {
        return Success(data);
    }

    public static implicit operator T(Result<T> result)
    {
        if (!result.IsSuccess || result.Data == null)
            throw new InvalidOperationException("Cannot convert failed or null result to data.");
        return result.Data;
    }
}

public class Error
{
    public static IEnumerable<Error> None = [];

    public string Code { get; }
    public string Message { get; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }
}

public record ResponseDto<T>(
    HttpStatusCode StatusCode, string? Message,
    IEnumerable<Error>? Errors, Pagination? Pagination, T? Data)
{
    public static ResponseDto<T> Success(string message = "",
        Pagination? pagination = null, T? data = default, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new ResponseDto<T>(statusCode, message, Error.None, pagination, data);
    }

    public static ResponseDto<T> Failure(IEnumerable<Error> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new ResponseDto<T>(statusCode, default, errors, default, default);
    }
}

public record Pagination(int PageNumber, int PageSize, int TotalRecords, int TotalPages);