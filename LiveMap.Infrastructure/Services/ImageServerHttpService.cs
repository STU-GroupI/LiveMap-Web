using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Infrastructure.Services
{
    public class ImageServerHttpService : IImageServerService
    {
        private readonly IBackendApiHttpService _backendApiService;
        private const string _ENDPOINT = "/apiuwu";

        public ImageServerHttpService(IBackendApiHttpService backendApiService)
        {
            _backendApiService = backendApiService;
        }

        public async Task<BackendApiHttpResponse<string>> Create(Image image)
        {
            return await _backendApiService
                .SendRequest<string>(new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    Content = new StringContent(image.Base64Image),
                    RequestUri = new Uri($"{_ENDPOINT}/upload")
                });
        }
    }
}
