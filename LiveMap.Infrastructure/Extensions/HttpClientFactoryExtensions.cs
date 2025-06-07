namespace LiveMap.Infrastructure.Extensions;
public static class IHttpClientFactoryExtensions
{
    public const string BackendClientName = "BackendApi";
    public static HttpClient? CreateBackendClient(this IHttpClientFactory httpClientFactory)
        => httpClientFactory.CreateClient(BackendClientName);

    public const string ImageClientName = "ImageServer";
    public static HttpClient? CreateImageClient(this IHttpClientFactory httpClientFactory)
        => httpClientFactory.CreateClient(ImageClientName);
}
