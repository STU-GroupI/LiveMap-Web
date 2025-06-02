using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using System.Text.Json;

namespace LiveMap.Infrastructure.Services;

public abstract class ExternalHttpServiceBase
{
    private readonly HttpClient _httpClient;

    public ExternalHttpServiceBase(HttpClient? client, TimeSpan timeout)
    {
        _httpClient = client ?? throw new NullReferenceException("Default http client may not be null");
        _httpClient.Timeout = timeout;
    }

    public async Task<ExternalHttpResponse<TResult>> SendRequest<TResult>(HttpRequestMessage request)
        where TResult : class
    {
        try
        {
            var result = await _httpClient.SendAsync(request);

            if (result.IsSuccessStatusCode)
            {

                var contentStream = await result.Content.ReadAsStreamAsync();
                if (contentStream is null || contentStream.Length == 0)
                {
                    return ExternalHttpResponse<TResult>.Success(
                        statusCode: result.StatusCode,
                        value: null
                    );
                }
                string temp = await result.Content.ReadAsStringAsync();
                TResult? data = await JsonSerializer.DeserializeAsync<TResult>(contentStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                return ExternalHttpResponse<TResult>.Success(
                    statusCode: result.StatusCode,
                    value: data
                );
            }

            string message = (await result.Content.ReadAsStringAsync()) ?? string.Empty;

            (string, string) errorMessage = (
                            result.ReasonPhrase ?? string.Empty,
                            message);

            return ExternalHttpResponse<TResult>.Failure(
                        statusCode: result.StatusCode,
                        value: null,
                        errorMessage: errorMessage);
        }
        catch (HttpRequestException ex)
        {
            return ExternalHttpResponse<TResult>.Failure(
                        statusCode: ex.StatusCode,
                        value: null,
                        errorMessage: (
                            "An error occured while making the request",
                            ex.Message
                        ));
        }
    }
    public async Task<ExternalHttpResponse> SendRequest(HttpRequestMessage request)
    {
        try
        {
            var result = await _httpClient.SendAsync(request);
            var responseContent = await result.Content.ReadAsStreamAsync();

            if (result.IsSuccessStatusCode)
            {
                return ExternalHttpResponse.Success(
                    statusCode: result.StatusCode
                );

            }

            return ExternalHttpResponse.Failure(
                statusCode: result.StatusCode,
                errorMessage: (
                    result.ReasonPhrase
                        ?? string.Empty,
                    await JsonSerializer.DeserializeAsync<string>(responseContent)
                        ?? string.Empty));
        }
        catch (HttpRequestException ex)
        {
            return ExternalHttpResponse.Failure(
                statusCode: ex.StatusCode,
                errorMessage: (
                    "An error occured while making the request",
                    ex.Message
                ));
        }
    }
}
