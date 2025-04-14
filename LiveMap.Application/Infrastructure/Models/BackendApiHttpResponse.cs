using System.Net;

namespace LiveMap.Application.Infrastructure.Models;

// TODO: Make this so there is always an expected resposne.
// You should always expect content. You could get a URL with a callback in it after, for instance, an update.

public sealed record BackendApiHttpResponse
{
    public readonly HttpStatusCode? StatusCode;
    public readonly bool IsSuccess;
    public readonly (string Phrase, string Message)? ErrorMessage;

    private BackendApiHttpResponse(
        HttpStatusCode? StatusCode,
        bool IsSuccess,
        (string Phrase, string Message)? ErrorMessage)
    {
        this.StatusCode = StatusCode;
        this.IsSuccess = IsSuccess;
        this.ErrorMessage = ErrorMessage;
    }

    public static BackendApiHttpResponse Success(
        HttpStatusCode? statusCode)
    {
        return new BackendApiHttpResponse(
            StatusCode: statusCode,
            IsSuccess: true,
            ErrorMessage: null
        );
    }

    public static BackendApiHttpResponse Failure(
        HttpStatusCode? statusCode,
        (string Phrase, string Message)? errorMessage)
    {
        return new BackendApiHttpResponse(
            StatusCode: statusCode,
            IsSuccess: false,
            ErrorMessage: errorMessage
        );
    }
};

public sealed record BackendApiHttpResponse<T>
{
    public readonly HttpStatusCode? StatusCode;
    public readonly bool IsSuccess;
    public readonly T? Value;
    public readonly (string Phrase, string Message)? ErrorMessage;

    private BackendApiHttpResponse(
        HttpStatusCode? StatusCode,
        bool IsSuccess,
        T? Value,
        (string Phrase, string Message)? ErrorMessage)
    {
        this.StatusCode = StatusCode;
        this.IsSuccess = IsSuccess;
        this.Value = Value;
        this.ErrorMessage = ErrorMessage;
    }

    public static BackendApiHttpResponse<T> Success(
        HttpStatusCode? statusCode,
        T? value)
    {
        return new BackendApiHttpResponse<T>(
            StatusCode: statusCode,
            IsSuccess: true,
            Value: value,
            ErrorMessage: null
        );
    }

    public static BackendApiHttpResponse<T> Failure(
        HttpStatusCode? statusCode,
        T? value,
        (string Phrase, string Message)? errorMessage)
    {
        return new BackendApiHttpResponse<T>(
            StatusCode: statusCode,
            IsSuccess: false,
            Value: value,
            ErrorMessage: errorMessage
        );
    }
}