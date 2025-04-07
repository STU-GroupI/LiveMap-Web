namespace LiveMapDashboard.Web.Extensions
{
    public static class IHttpClientFactoryExtensions
    {
        public const string BackendClientName = "BackendApi";
        public static HttpClient? CreateBackendClient(this IHttpClientFactory httpClientFactory) 
            => httpClientFactory.CreateClient(BackendClientName);
    }
}
