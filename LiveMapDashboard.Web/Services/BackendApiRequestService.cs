using LiveMapDashboard.Web.Extensions;
using System;
using static System.Collections.Specialized.BitVector32;
using System.Net;
using System.Text.Json;
using Bogus.Bson;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;

namespace LiveMapDashboard.Web.Services;

// At this point, could consider making these abstract and providing factories for them.
// If they prove to add to much complexity, abstract it away and simplify the base usage...
public sealed record BackendApiResponse<T>
{
    public readonly HttpStatusCode? StatusCode;
    public readonly bool IsSuccess;
    public readonly T? Value;
    public readonly (string Phrase, string Message)? ErrorMessage;

    private BackendApiResponse(
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

    public static BackendApiResponse<T> Success(
        HttpStatusCode? statusCode,
        T? value)
    {
        return new BackendApiResponse<T>(
            StatusCode: statusCode,
            IsSuccess: true,
            Value: value,
            ErrorMessage: null
        );
    }

    public static BackendApiResponse<T> Failure(
        HttpStatusCode? statusCode,
        T? value,
        (string Phrase, string Message)? errorMessage)
    {
        return new BackendApiResponse<T>(
            StatusCode: statusCode,
            IsSuccess: false,
            Value: value,
            ErrorMessage: errorMessage
        );
    }
}

public sealed record BackendApiResponse
{
    public readonly HttpStatusCode? StatusCode;
    public readonly bool IsSuccess;
    public readonly (string Phrase, string Message)? ErrorMessage;

    private BackendApiResponse(
        HttpStatusCode? StatusCode,
        bool IsSuccess,
        (string Phrase, string Message)? ErrorMessage)
    {
        this.StatusCode = StatusCode;
        this.IsSuccess = IsSuccess;
        this.ErrorMessage = ErrorMessage;
    }

    public static BackendApiResponse Success(
        HttpStatusCode? statusCode)
    {
        return new BackendApiResponse(
            StatusCode: statusCode,
            IsSuccess: true,
            ErrorMessage: null
        );
    }

    public static BackendApiResponse Failure(
        HttpStatusCode? statusCode,
        (string Phrase, string Message)? errorMessage)
    {
        return new BackendApiResponse(
            StatusCode: statusCode,
            IsSuccess: false,
            ErrorMessage: errorMessage
        );
    }
};

public interface IBackendApiRequestService
{
    BackendApiRequestService ConfigureClient(Action<HttpClient> action);
    Task<BackendApiResponse> SendRequest(HttpRequestMessage request);
    Task<BackendApiResponse<TResult>> SendRequest<TResult>(HttpRequestMessage request) where TResult : class;
}

public class BackendApiRequestService : IBackendApiRequestService
{
    private readonly HttpClient _httpClient;

    public BackendApiRequestService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateBackendClient()
            ?? throw new NullReferenceException("Default backend http client may not be null");
    }

    public BackendApiRequestService ConfigureClient(Action<HttpClient> action)
    {
        action(_httpClient);
        return this;
    }

    public async Task<BackendApiResponse<TResult>> SendRequest<TResult>(HttpRequestMessage request)
        where TResult : class
    {
        try
        {
            var result = await _httpClient.SendAsync(request);
            var responseContent = await result.Content.ReadAsStreamAsync();

            if (result.IsSuccessStatusCode)
            {
                if (responseContent is null)
                {
                    return BackendApiResponse<TResult>.Success(
                        statusCode: result.StatusCode,
                        value: null
                    );
                }

                TResult? data = await JsonSerializer.DeserializeAsync<TResult>(responseContent);

                return BackendApiResponse<TResult>.Success(
                    statusCode: result.StatusCode,
                    value: data
                );

            }
            (string, string) errorMessage = (
                            result.ReasonPhrase
                                ?? string.Empty,
                            await JsonSerializer.DeserializeAsync<string>(responseContent)
                                ?? string.Empty);

            return BackendApiResponse<TResult>.Failure(
                        statusCode: result.StatusCode,
                        value: null,
                        errorMessage: errorMessage);
        }
        catch (HttpRequestException ex)
        {
            return BackendApiResponse<TResult>.Failure(
                        statusCode: ex.StatusCode,
                        value: null,
                        errorMessage: (
                            "An error occured while making the request",
                            ex.Message
                        ));
        }
    }

    public async Task<BackendApiResponse> SendRequest(HttpRequestMessage request)
    {
        try
        {
            var result = await _httpClient.SendAsync(request);
            var responseContent = await result.Content.ReadAsStreamAsync();

            if (result.IsSuccessStatusCode)
            {
                if (responseContent is null)
                {
                    return BackendApiResponse.Success(
                        statusCode: result.StatusCode
                    );
                }

                return BackendApiResponse.Success(
                    statusCode: result.StatusCode
                );
            }

            return BackendApiResponse.Failure(
                statusCode: result.StatusCode,
                errorMessage: (
                    result.ReasonPhrase
                        ?? string.Empty,
                    await JsonSerializer.DeserializeAsync<string>(responseContent)
                        ?? string.Empty));
        }
        catch (HttpRequestException ex)
        {
            return BackendApiResponse.Failure(
                statusCode: ex.StatusCode,
                errorMessage: (
                    "An error occured while making the request",
                    ex.Message
                ));
        }
    }
}
