using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using System.Text;
using System.Text.Json;

namespace LiveMap.Infrastructure.Services
{
    public class ImageHttpService : IImageService
    {
        private readonly IImageServerHttpService _imageHttpService;
        private const string _ENDPOINT = "image";

        public ImageHttpService(IImageServerHttpService imageHttpService)
        {
            _imageHttpService = imageHttpService;
        }

        public async Task<ExternalHttpResponse<string>> Create(Image image)
        {
            return await _imageHttpService
                .SendRequest<string>(new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    Content = new StringContent(JsonSerializer.Serialize(new { image }), Encoding.UTF8, "application/json"),
                    RequestUri = new Uri($"{_ENDPOINT}", UriKind.Relative)
                });
        }
    }
}
