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
    public class ImageHttpService : IImageService
    {
        private readonly IImageServerHttpService _imageHttpService;
        private const string _ENDPOINT = "";

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
                    Content = new StringContent(image.Base64Image),
                    RequestUri = new Uri($"{_ENDPOINT}/upload")
                });
        }
    }
}
