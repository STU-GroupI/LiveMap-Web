using LiveMapDashboard.Web.Extensions;
using System;
using static System.Collections.Specialized.BitVector32;
using System.Net;
using System.Text.Json;

namespace LiveMapDashboard.Web.Services;

// At this point, could consider making these abstract and providing factories for them.
// If they prove to add to much complexity, abstract it away and simplify the base usage...
public sealed record BackendApiResponse<T>(
    HttpStatusCode? StatusCode,
    bool IsSuccess,
    T? Value,
    (string Phrase, string Message)? ErrorMessage
);

public sealed record BackendApiResponse(
    HttpStatusCode? StatusCode,
    bool IsSuccess,
    (string Phrase, string Message)? ErrorMessage
);

public class BackendApiRequestService
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
                    return new BackendApiResponse<TResult>(
                        StatusCode: result.StatusCode,
                        IsSuccess: result.IsSuccessStatusCode,
                        Value: null,
                        ErrorMessage: null
                    );
                }

                TResult? data = await JsonSerializer.DeserializeAsync<TResult>(responseContent);
                return new BackendApiResponse<TResult>(
                        StatusCode: result.StatusCode,
                        IsSuccess: result.IsSuccessStatusCode,
                        Value: data,
                        ErrorMessage: null
                );
            }

            return new BackendApiResponse<TResult>(
                        StatusCode: result.StatusCode,
                        IsSuccess: result.IsSuccessStatusCode,
                        Value: null,
                        ErrorMessage: (
                            result.ReasonPhrase
                                ?? string.Empty,
                            await JsonSerializer.DeserializeAsync<string>(responseContent) 
                                ?? string.Empty));
        }
        catch (HttpRequestException ex)
        {
            return new BackendApiResponse<TResult>(
                        StatusCode: ex.StatusCode,
                        IsSuccess: false,
                        Value: null,
                        ErrorMessage: (
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
                    return new BackendApiResponse(
                        StatusCode: result.StatusCode,
                        IsSuccess: result.IsSuccessStatusCode,
                        ErrorMessage: null
                    );
                }

                return new BackendApiResponse(
                        StatusCode: result.StatusCode,
                        IsSuccess: result.IsSuccessStatusCode,
                        ErrorMessage: null
                );
            }

            return new BackendApiResponse(
                        StatusCode: result.StatusCode,
                        IsSuccess: result.IsSuccessStatusCode,
                        ErrorMessage: (
                            result.ReasonPhrase
                                ?? string.Empty,
                            await JsonSerializer.DeserializeAsync<string>(responseContent)
                                ?? string.Empty));
        }
        catch (HttpRequestException ex)
        {
            return new BackendApiResponse(
                        StatusCode: ex.StatusCode,
                        IsSuccess: false,
                        ErrorMessage: (
                            "An error occured while making the request",
                            ex.Message
                        ));
        }
    }
}
