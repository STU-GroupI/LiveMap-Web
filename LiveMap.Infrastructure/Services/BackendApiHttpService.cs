using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using LiveMap.Infrastructure.Extensions;
using System.Net.Http;
using System.Text.Json;

namespace LiveMap.Infrastructure.Services;

public class BackendApiHttpService : IBackendApiHttpService
{
    private readonly HttpClient _httpClient;

    public BackendApiHttpService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateBackendClient()
            ?? throw new NullReferenceException("Default backend http client may not be null");
    }

    public async Task<BackendApiHttpResponse<TResult>> SendRequest<TResult>(HttpRequestMessage request)
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
                    return BackendApiHttpResponse<TResult>.Success(
                        statusCode: result.StatusCode,
                        value: null
                    );
                }

                TResult? data = await JsonSerializer.DeserializeAsync<TResult>(responseContent);

                return BackendApiHttpResponse<TResult>.Success(
                    statusCode: result.StatusCode,
                    value: data
                );
            }

            string message = responseContent.Length > 0
                ? await JsonSerializer.DeserializeAsync<string>(responseContent) ?? string.Empty
                : string.Empty;


            (string, string) errorMessage = (
                            result.ReasonPhrase ?? string.Empty,
                            message);

            return BackendApiHttpResponse<TResult>.Failure(
                        statusCode: result.StatusCode,
                        value: null,
                        errorMessage: errorMessage);
        }
        catch (HttpRequestException ex)
        {
            return BackendApiHttpResponse<TResult>.Failure(
                        statusCode: ex.StatusCode,
                        value: null,
                        errorMessage: (
                            "An error occured while making the request",
                            ex.Message
                        ));
        }
    }
    public async Task<BackendApiHttpResponse> SendRequest(HttpRequestMessage request)
    {
        try
        {
            var result = await _httpClient.SendAsync(request);
            var responseContent = await result.Content.ReadAsStreamAsync();

            if (result.IsSuccessStatusCode)
            {
                return BackendApiHttpResponse.Success(
                    statusCode: result.StatusCode
                );

            }

            return BackendApiHttpResponse.Failure(
                statusCode: result.StatusCode,
                errorMessage: (
                    result.ReasonPhrase
                        ?? string.Empty,
                    await JsonSerializer.DeserializeAsync<string>(responseContent)
                        ?? string.Empty));
        }
        catch (HttpRequestException ex)
        {
            return BackendApiHttpResponse.Failure(
                statusCode: ex.StatusCode,
                errorMessage: (
                    "An error occured while making the request",
                    ex.Message
                ));
        }
    }
}
